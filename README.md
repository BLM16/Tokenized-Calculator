Tokenized Calculator
====================

[![C#](https://img.shields.io/static/v1?label=C%23&message=v11&color=brightgreen)](https://docs.microsoft.com/en-us/dotnet/) [![License](https://img.shields.io/badge/license-MIT-blue.svg?label=License)](https://github.com/BLM16/Tokenized-Calculator/blob/master/LICENSE) [![Nuget](https://img.shields.io/nuget/v/BLM16.Util.Calculator?label=Nuget&logo=nuget)](https://www.nuget.org/packages/BLM16.Util.Calculator/)

This library parses and solves math equations from strings. Order of Operations Rules are followed.

---

**Note:** multiple consecutive exponents are evaluated as (a^b)^c not a^(b^c). Using brackets to explicitly outline order is advised.

**Note:** constants and functions are evaluated by length of symbol. If `pi` and `p` are defined, `pi` evaluates to `pi` not `p * i`. Use operators or parentheses to be explicit about which constants to use.

**Note:** operators, constants, and functions are case-insensitive (`Pi = pI = PI = pi`).

---

Predefined operators, constants, and functions exist for your convenience. Not all of them used by default however.
In the calculator's initializer you can add things from DefaultOperators, DefaultConstants, DefaultFunctions, or your own custom values.

## License
This code is licensed under the [MIT License](https://github.com/BLM16/Tokenized-Calculator/blob/master/LICENSE)
