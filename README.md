# Modulus Checker <img src="https://ci.appveyor.com/api/projects/status/qihofc0xk80vk0to?svg=true">

This is a C# implementation of UK Bank Account Modulus Checking. Modulus Checking is a process used to determine if a given account number could be valid for a given sort code.
***
Receiving a valid modulus check result only means that the Sort Code and Account Number pair **could** exist not that they do!
***
The algorithms, test cases and reference data can be found  [on the vocalink website](http://www.vocalink.com/products/payments/customer-support-services/modulus-checking.aspx "The Vocalink Modulus Checker Website"). That site should be considered the authoritative source for the modulus checking algorithm and weighting data.

#### Nuget
Modulus Checker is [available on Nuget](https://nuget.org/packages/ModulusChecker/). To install it run the following command in the Package Manager Console ```Install-Package ModulusChecker``` and reference its namespace as ```using ModulusChecking;```

#### Version Requirements

Prior to version 2.0.0 ModulusChecker support .Net 3.5, 4.0, and 4.5

From 2.0.0 onwards it only supports .Net 4.6.2

#### Usage
```
var sortCode = "012345";
var accountNumber = "12345678";
var modulusChecker = new ModulusChecker();
var result = modulusChecker
			.CheckBankAccount(sortCode,accountNumber);
```
If looping over a number of bank account details it is not necessary to initialise the ModulusChecker between checks.

```
var things = new List<BankAccountDetails> { 
  //some items
}; 
var modulusChecker = new ModulusChecker();
var results = things.map(t => 
  modulusChecker.CheckBankAccount(thing.sortCode, thing.accountNumber));
```

#### Explanation Mode

```
const string sortCode = "107999";
const string accountNumber = "88837493";
var modulusChecker = new ModulusChecker();
var outcome = modulusChecker.CheckBankAccountWithExplanation(sortCode,accountNumber);
            
Assert.AreEqual(false, outcome.Result);
Assert.AreEqual("not proceeding to the second check as there is only one weight mapping", outcome.Explanation);

```

or when the sort code is not covered by the modulus checking algorithm 

```
const string sortCode = "000000";
const string accountNumber = "88837493";
var modulusChecker = new ModulusChecker();
var outcome = modulusChecker.CheckBankAccountWithExplanation(sortCode,accountNumber);
            
Assert.AreEqual(true, outcome.Result);
Assert.AreEqual("Cannot invalidate these account details as there are no weight mappings for this sort code", outcome.Explanation);
```

#### Valacdos File Contents

The library is built with a fixed version of the valacdos files (see below).

As of v1.9.0 and v2.4.0 the valacdos files can be provided to the modulus checker constructor. 

```
const string weightMappingsContent = "blah blah";
const string scsubtabContent = "blah blah blah";
var modulusChecker = new ModulusChecker(weightMappingsContent, scsubtabContent);
var outcome = modulusChecker.CheckBankAccountWithExplanation(sortCode,accountNumber);
            
Assert.AreEqual(false, outcome.Result);
Assert.AreEqual("not proceeding to the second check as there is only one weight mapping", outcome.Explanation);

```

Versions 4 to 5.2 of the spec haven't included any changes to the modulus checking algorithm or tests just alterations to the data in the valacdos files. But this can't be guaranteed for any released version. 

If you want to provide the valacdos contents to the checker you do need to check what changes are in the new version to be sure that the data files will work as expected.

##### How to provide the contents

The library accepts the string content of the two valacdos files and not the paths to the files. It is best to read the files only once and construct the ModulusChecker once in your application's [composition root](http://blog.ploeh.dk/2011/07/28/CompositionRoot/)  

#### License
This software is released under the MIT license. 

NB the resource text files valacdos.txt and scsubtab.txt are produced and released by Vocalink not me

#### Vocalink Version

Currently defaults to v5.20 of Vocalink Modulus Checking copied from [their site](https://www.vocalink.com/customer-support/modulus-checking/) on 2018-10-22