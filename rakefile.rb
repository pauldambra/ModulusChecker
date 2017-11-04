
NUGET_PATH = "./Build/Nuget.exe"
NUNIT_RUNNER = "./packages/NUnit.Runners.2.6.4/tools/nunit-console.exe"
MODULUS_CHECKING_TESTS_CSPROJ = './ModulusCheckingTests/ModulusCheckingTests.csproj'

default_build_command = 'MSBuild'
BUILD_COMMAND = ENV['BUILD_COMMAND'] || default_build_command
BUILD_CONFIG = ENV['Configuration'] || 'Release'

BUILD_NUMBER = ENV['APPVEYOR_BUILD_NUMBER'] || '0'
version = '0.0.0'

task :versioning do
  file_version = File.readlines('./.semver')[0].chomp
  version = "#{file_version}.#{BUILD_NUMBER}"
  puts "version is #{version}"

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
  :tests
  # :create_nugets
]

task :appveyor => [
  :versioning,
  :restore,
  :clean,
  :build
  # :create_nugets
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

task clean: [:clean_packages] do 
  run_msbuild 'Clean' 
end

task :build do 
  run_msbuild 'Build'
end

def run_tests(files)
  out_file ='/result=TestResults.xml'
  sh "#{NUNIT_RUNNER} #{files} #{out_file}"
end

task :tests do
    run_tests FileList['./*Tests/bin/**/*Tests.dll'].join(' ')
end

# directory 'nuget'

# nugets_pack :create_nugets do |p|
#   puts "packaging nuget version: #{version}"

#   p.files   = FileList['./ModulusChecking/ModulusChecking.csproj']
#   p.out     = './Build/appveyor_artifacts'
#   p.exe     = NUGET_PATH
#   p.with_metadata do |m|
#     m.description = 'This is a C# implementation of UK Bank Account Modulus Checking. Modulus Checking is a process used to determine if a given account number could be valid for a given sort code.'
#     m.authors = "Paul D'Ambra"
#     m.version = ENV["NUGET_VERSION"]
#     m.tags = "C# BankAccount ModulusChecking ModulusChecker Modulus Direct Debit UK"
#   end
#   p.with_package do |p|
#     p.add_file 'bin/Release-netv45/ModulusChecker.dll', 'lib/net45'
#   end
#    p.leave_nuspec
# end
