require 'albacore'
require 'albacore/tasks/versionizer'

::Albacore::Tasks::Versionizer.new :versioning

NUGET_PATH = "./Nuget.exe"
NUNIT_RUNNER = "packages/NUnit.Runners.2.6.3/tools/nunit-console.exe"

# task :print_versions => :versioning do
# 	puts ENV["BUILD_VERSION"]
# 	puts ENV["NUGET_VERSION"]
# 	puts ENV["FORMAL_VERSION"]
# end

desc 'restore all nugets as per the packages.config files'
nugets_restore :restore do |p|
  p.out = 'nuget'
  p.exe = NUGET_PATH
end

desc 'Perform full build'
build :build => [:versioning, :restore] do |b|
  b.sln = '../ModulusChecking.sln'
end

test_runner :tests do |tests|
  	tests.files = FileList['**/*Tests/bin/Release/*Tests.dll']
    tests.add_parameter '/TestResults=TestResults.xml'
  	tests.exe = NUNIT_RUNNER
end

directory 'nuget'

desc 'package nugets - finds all projects and package them'
nugets_pack :create_nugets => ['nuget', :versioning, :build] do |p|
  p.files   = FileList['../ModulusChecking/ModulusChecking.csproj']
  p.out     = 'nuget'
  p.exe     = NUGET_PATH
  p.with_metadata do |m|
    m.description = 'This is a C# implementation of UK Bank Account Modulus Checking. Modulus Checking is a process used to determine if a given account number could be valid for a given sort code.'
    m.authors = "Paul D'Ambra"
    m.version = ENV["NUGET_VERSION"]
    m.tags = "C# BankAccount ModulusChecking ModulusChecker Modulus Direct Debit UK"
  end
  p.with_package do |p|
    p.add_file 'bin/Release/ModulusChecker.dll', 'lib/net45'
  end
end

task :default => :create_nugets