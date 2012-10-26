# Modulus Checker

This is a C# implementation of UK Bank Account Modulus Checking. Modulus Checking is a process used to determine if a given account number could be valid for a given sort code.
***
Receiving a valid modulus check result only means that the Sort Code and Account Number pair **could** exist not that they do!
***
The algorithms, test cases and reference data can be found  [on the vocalink website](http://www.vocalink.com/products/payments/customer-support-services/modulus-checking.aspx "The Vocalink Modulus Checker Website"). That site should be considered the authoritative source for the modulus checking algorithm and weighting data.


#### Usage
```csharp
var sortCode = "012345";
var accountNumber = "12345678";
var modulusChecker	= new ModulusChecker();
var result =  modulusChecker
			.CheckBankAccount(sortCode,accountNumber);
```
If looping over a number of bank account details it is not necessary to initialise the ModulusChecker between checks.

```csharp
var sortCode = "012345";
var accountNumber = "12345678";
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

#### To Do
* Improve recognition of sort codes of banks with 10 digit account numbers
* Explicitly test thread safety
