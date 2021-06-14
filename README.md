
# Flattening generator

The generator allows you to generate flattened classes from rich domain classes.

## Installation

```bash
  dotnet add package Kros.Generators.Flattening
```

## How to use it

Sometimes you need to create a class from your domain that has flattened properties (does not contain composite). For example, the class `Person` contains the `Address` property and the `Town` property. But you would need a class with the `AddressTown` property.

You define partial class and marke it with the `Flatten` attribute.

```csharp
[Flatten(SourceType = typeof(Person))]
public partial class PersonFlat
{
}
```

And that's it. The generator will generate a partial class for you, which will contain all the necessary properties.

## Another example

You have the following domain classes and value objects.

```csharp
public class Document
{
    public Contact Owner { get; set; }

    public Contact Collaborator { get; set; }

    public string Name { get; set; }
}

public class Contact
{
    public string Name { get; set; }

    public Address Address { get; set; }
}

public class Address
{
    public string City { get; set; }
    public string Street { get; set; }
}
```

And you want a flattened class, but with some exceptions.

```csharp
[Flatten(SourceType = typeof(Document))]
[FlattenPropertyName(SourcePropertyName = "Owner.Address.City", Name = "Town")]
[FlattenPropertyName(SourcePropertyName = "Collaborator.Address", Name = "")]
public partial class DocumentFlat
{
}
```

The generated class will look like this:

```csharp
public partial class DocumentFlat
{
    public string OwnerName { get; set; }

    public string OwnerAddressTown { get; set; }

    public string OwnerAddressStreet { get; set; }

    public string CollaboratorName { get; set; }

    public string CollaboratorCity { get; set; }

    public string CollaboratorStreet { get; set; }

    public string Name { get; set; }
}
```

## Settings

### Skip

You may not want to generate some properties into the output flattened class. To do this, you can use the `Skip` parameter and define a list of properties to be ignored.

```csharp
[Flatten(SourceType = typeof(Document),
    Skip = new string[] { nameof(Document.Collaborator)})]
```

### DoNotFlatten

If you do not want to flatten any of the properties, use the parameter to do so `DoNotFlatten`.

```csharp
[Flatten(SourceType = typeof(Document),
    DoNotFlatten = new string[] { nameof(Document.Owner) })]
```

### Rename outup property

If you want to rename the output property, use the `FlattenPropertyName` attribute.

```csharp
[FlattenPropertyName(SourcePropertyName = "Owner.Address.City", Name = "Town")]
[FlattenPropertyName(SourcePropertyName = "Collaborator.Address", Name = "")]
```
