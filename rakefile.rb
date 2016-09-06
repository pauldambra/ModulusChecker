require 'albacore'
require 'albacore/tasks/versionizer'

::Albacore::Tasks::Versionizer.new :versioning

NUGET_PATH = "./Build/Nuget.exe"
NUNIT_RUNNER = "./packages/NUnit.Runners.2.6.4/tools/nunit-console.exe"

MOQ_35_PATTERN =  /\<HintPath\>\.\.\\packages\\Moq\.4\.0\.10827\\lib\\NET35\\Moq\.dll\<\/HintPath\>/
MOQ_35_HINT = '<HintPath>..\packages\Moq.4.0.10827\lib\NET35\Moq.dll</HintPath>'
MOQ_40_PATTERN =  /\<HintPath\>\.\.\\packages\\Moq\.4\.0\.10827\\lib\\NET40\\Moq\.dll\<\/HintPath\>/
MOQ_40_HINT = '<HintPath>..\packages\Moq.4.0.10827\lib\NET40\Moq.dll</HintPath>'
MODULUS_CHECKING_TESTS_CSPROJ = './ModulusCheckingTests/ModulusCheckingTests.csproj'
# task :print_versions => :versioning do
# 	puts ENV["BUILD_VERSION"]
# 	puts ENV["NUGET_VERSION"]
# 	puts ENV["FORMAL_VERSION"]
# end

task :dot_net_35 => [:build_35, :tests_35]
task :dot_net_4 => [:build_40, :tests_40]
task :dot_net_45 => [:build_45, :tests_45]
task :default => [
  :versioning,
  :restore,
  :clean,
  :dot_net_35,
  :dot_net_4,
  :dot_net_45,
  :create_nugets
]

nugets_restore :restore do |p|
  p.out = 'nuget'
  p.exe = NUGET_PATH
end

build :clean do |b|
  b.sln = './ModulusChecking.sln'
  b.target = 'Clean'
end

task :use_moq_35 do
  IO.write(MODULUS_CHECKING_TESTS_CSPROJ, File.open(MODULUS_CHECKING_TESTS_CSPROJ) do |f|
    f.read.gsub(MOQ_40_PATTERN, MOQ_35_HINT)
  end)
end

task :use_moq_40 do
  IO.write(MODULUS_CHECKING_TESTS_CSPROJ, File.open(MODULUS_CHECKING_TESTS_CSPROJ) do |f|
    f.read.gsub(MOQ_35_PATTERN, MOQ_40_HINT)
  end)
end


build :build_35 => [:use_moq_35] do |b|
  b.sln = './ModulusChecking.sln'
  b.prop 'TargetFrameworkVersion', 'v3.5'
  b.prop 'outdir', 'bin/Release-netv35/'
  b.logging = 'normal'
  b.tools_version = 3.5    
  b.prop 'Configuration', 'Release'
end

build :build_40 => [:use_moq_40] do |b|
  b.sln = './ModulusChecking.sln'
  b.prop 'TargetFrameworkVersion', 'v4.0'
  b.prop 'outdir', 'bin/Release-netv4/'
  b.prop 'Configuration', 'Release'
end

build :build_45 => [:use_moq_40] do |b|
  b.sln = './ModulusChecking.sln'
  b.prop 'TargetFrameworkVersion', 'v4.5'
  b.prop 'outdir', 'bin/Release-netv45/'
  b.prop 'Configuration', 'Release'
end

def run_tests(files)
  out_file ='/result=TestResults.xml'
  sh "#{NUNIT_RUNNER} #{files} #{out_file}"
end

task :tests_35 do
  run_tests FileList['./*Tests/bin/Release-netv35/*Tests.dll'].join(' ')
end

task :tests_40 do
    run_tests FileList['./*Tests/bin/Release-netv4/*Tests.dll'].join(' ')
end

task :tests_45 do
    run_tests FileList['./*Tests/bin/Release-netv45/*Tests.dll'].join(' ')
end

directory 'nuget'

desc 'package nugets - finds all projects and package them'
nugets_pack :create_nugets do |p|
  puts ENV["NUGET_VERSION"]

  p.files   = FileList['./ModulusChecking/ModulusChecking.csproj']
  p.out     = 'nuget'
  p.exe     = NUGET_PATH
  p.with_metadata do |m|
    m.description = 'This is a C# implementation of UK Bank Account Modulus Checking. Modulus Checking is a process used to determine if a given account number could be valid for a given sort code.'
    m.authors = "Paul D'Ambra"
    m.version = ENV["NUGET_VERSION"]
    m.tags = "C# BankAccount ModulusChecking ModulusChecker Modulus Direct Debit UK"
  end
  p.with_package do |p|
    p.add_file 'bin/Release-netv35/ModulusChecker.dll', 'lib/net35'
    p.add_file 'bin/Release-netv4/ModulusChecker.dll', 'lib/net40'
    p.add_file 'bin/Release-netv45/ModulusChecker.dll', 'lib/net45'
  end
end
