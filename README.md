# Modulus Checker <img src="https://ci.appveyor.com/api/projects/status/qihofc0xk80vk0to?svg=true">

This is a C# implementation of UK Bank Account Modulus Checking. Modulus Checking is a process used to determine if a given account number could be valid for a given sort code.
***
Receiving a valid modulus check result only means that the Sort Code and Account Number pair **could** exist not that they do!
***
The algorithms, test cases and reference data can be found  [on the vocalink website](http://www.vocalink.com/products/payments/customer-support-services/modulus-checking.aspx "The Vocalink Modulus Checker Website"). That site should be considered the authoritative source for the modulus checking algorithm and weighting data.

#### Nuget
Modulus Checker is [available on Nuget](https://nuget.org/packages/ModulusChecker/). To install it run the following command in the Package Manager Console ```Install-Package ModulusChecker``` and reference its namespace as ```using ModulusChecking;```


#### Usage
```
var sortCode = "012345";
var accountNumber = "12345678";
var modulusChecker	= new ModulusChecker();
var result =  modulusChecker
			.CheckBankAccount(sortCode,accountNumber);
```
If looping over a number of bank account details it is not necessary to initialise the ModulusChecker between checks.

```
var things = new List<BankAccountDetails> { 
  //some items
}; 
var modulusChecker	= new ModulusChecker();
foreach(var thing in things) 
{
	var result =  modulusChecker
			.CheckBankAccount(thing.sortCode,
							thing.accountNumber);
}
```
#### License
This software is released under the MIT license. 

NB the resource text files valacdos.txt and scsubtab.txt are produced and released by Vocalink not me

#### Vocalink Version

Currently uses v4.3 of Vocalink Modulus Checking copied from their site on 2017-03-07