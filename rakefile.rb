
NUGET_PATH = "./Build/nuget.exe"
NUNIT_RUNNER = "./packages/NUnit.ConsoleRunner.3.7.0/tools/nunit3-console.exe"
MODULUS_CHECKING_TESTS_CSPROJ = './ModulusCheckingTests/ModulusCheckingTests.csproj'

default_build_command = 'MSBuild'
BUILD_COMMAND = ENV['BUILD_COMMAND'] || default_build_command
BUILD_CONFIG = ENV['Configuration'] || 'Release'

BUILD_NUMBER = ENV['APPVEYOR_BUILD_NUMBER'] || '0'
version = '0.0.0'

task :versioning do
  file_version = File.readlines('./.semver')[0].chomp
  version = "#{file_version}.#{BUILD_NUMBER}"

  FileList['./**/Properties/AssemblyInfo.cs'].each do |assemblyfile|
    file = File.read(assemblyfile, encoding: Encoding::UTF_8)
    new_contents = file.gsub(/AssemblyVersion\("\d\.\d\.\d\.\d"\)/, "AssemblyVersion(\"#{version}\")")
                       .gsub(/AssemblyFileVersion\("\d\.\d\.\d\.\d"\)/, "AssemblyFileVersion(\"#{version}\")")
    File.open(assemblyfile, "w") {|f| f.puts new_contents }
  end
end

directory './Build/appveyor_artifacts'

task clean_appveyor_artifacts: ['./Build/appveyor_artifacts'] do
  FileUtils.rm_rf(Dir.glob('./Build/appveyor_artifacts/*'))
end

task :default => [
  :versioning,
  :restore,
  :clean,
  :build,
  :tests,
  :nuget_pack
]

task :appveyor => [
  :versioning,
  :restore,
  :clean,
  :build,
  :nuget_pack
]

task :restore do
  sh "#{NUGET_PATH} restore ModulusChecking.sln"
end

def run_msbuild(target)
  command = [
    BUILD_COMMAND,
    './ModulusChecking.sln',
    '/verbosity:minimal',
    "/property:configuration=\"#{BUILD_CONFIG}\"",
    '/property:VisualStudioVersion="14.0"',
    '/m',
    "/target:\"#{target}\"",
  ].join(' ')
  sh command
end

task clean: [:clean_appveyor_artifacts] do 
  run_msbuild 'Clean' 
end

task :build do 
  run_msbuild 'Build'
end

task :tests do
    files = FileList['./*Tests/bin/**/*Tests.dll'].join(' ')
    out_file ='/result=TestResults.xml'
    sh "#{NUNIT_RUNNER} #{files} #{out_file}"
end

task :nuget_pack do |p|
  puts "packaging nuget version: #{version}"

  command = [
    NUGET_PATH,
    'pack',
    './ModulusChecking/ModulusChecker.nuspec',
    '-OutputDirectory ./Build/appveyor_artifacts',
    "-Version #{version}"
  ].join(' ')

  sh command
end
