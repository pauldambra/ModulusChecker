require 'albacore'
require 'albacore/tasks/versionizer'

::Albacore::Tasks::Versionizer.new :versioning

NUGET_PATH = "./Build/Nuget.exe"
NUNIT_RUNNER = "./packages/NUnit.Runners.2.6.4/tools/nunit-console.exe"

MODULUS_CHECKING_TESTS_CSPROJ = './ModulusCheckingTests/ModulusCheckingTests.csproj'

# task :print_versions => :versioning do
# 	puts ENV["BUILD_VERSION"]
# 	puts ENV["NUGET_VERSION"]
# 	puts ENV["FORMAL_VERSION"]
# end

directory './Build/appveyor_artifacts'

task clean_appveyor_artifacts: ['./Build/appveyor_artifacts'] do
  FileUtils.rm_rf(Dir.glob('./Build/appveyor_artifacts/*'))
end

task :default => [
  :versioning,
  :restore,
  :clean,
  :build_46,
  :tests,
  :create_nugets
]

task :appveyor => [
  :versioning,
  :restore,
  :clean,
  :build_46,
  :create_nugets
]

task :restore do
  sh "#{NUGET_PATH} restore ModulusChecking.sln"
end

build clean: [:clean_appveyor_artifacts] do |b|
  b.sln = './ModulusChecking.sln'
  b.target = 'Clean'
end

build :build_46 do |b|
  b.sln = './ModulusChecking.sln'
  b.prop 'outdir', 'bin/Release-netv46/'
  b.prop 'Configuration', 'Release'
end

def run_tests(files)
  out_file ='/result=TestResults.xml'
  sh "#{NUNIT_RUNNER} #{files} #{out_file}"
end

task :tests do
    run_tests FileList['./*Tests/bin/Release-netv46/*Tests.dll'].join(' ')
end

directory 'nuget'

nugets_pack :create_nugets do |p|
  puts "packaging nuget version: #{ENV["NUGET_VERSION"]}"

  p.files   = FileList['./ModulusChecking/ModulusChecking.csproj']
  p.out     = './Build/appveyor_artifacts'
  p.exe     = NUGET_PATH
  p.with_metadata do |m|
    m.description = 'This is a C# implementation of UK Bank Account Modulus Checking. Modulus Checking is a process used to determine if a given account number could be valid for a given sort code.'
    m.authors = "Paul D'Ambra"
    m.version = ENV["NUGET_VERSION"]
    m.tags = "C# BankAccount ModulusChecking ModulusChecker Modulus Direct Debit UK"
  end
  p.with_package do |p|
    p.add_file 'bin/Release-netv45/ModulusChecker.dll', 'lib/net45'
  end
   p.leave_nuspec
end
