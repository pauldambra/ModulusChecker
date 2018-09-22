using System;

namespace ModulusChecking.Loaders
{
    public class ProvidedValacodosContentIsNull : ArgumentNullException {}
    public class ProvidedValacodosContentIsEmpty : ArgumentException {}
    public class ProvidedValacodosContentIsProbablyInvalid : Exception {}
}