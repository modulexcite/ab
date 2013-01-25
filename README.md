ab
==

* A split testing micro-framework for ASP.NET MVC, inspired by vanity *

Introduction
------------

In a post-agile world, we are asked to look beyond the technologies that enable our practice, and find ways to ensure the choices we make are informed by
customers and stand up to reality. Experiment-driven (or evidence-based) development is a way of combining run-time metrics with automated experiments, 
resulting in software that is “natural”, based on actual use and runtime performance rather than the strongest opinion.

This library fulfills the automated experiments aspect for practicing EDD in a .NET development environment.
If you're looking for the run-time metrics aspect of EDD, you can use [metrics-net](https://github.com/danielcrenna/metrics-net).

Requirements
------------
* .NET 4.0
* ASP.NET MVC 4

How To Use
----------
**First**, specify AB as a dependency:

```powershell
PM> Install-Package AB
```

**Second**, define your experiments:

```csharp
using ab;

public class ExperimentConfig
{
	public void Register()
	{

	}
}
```
