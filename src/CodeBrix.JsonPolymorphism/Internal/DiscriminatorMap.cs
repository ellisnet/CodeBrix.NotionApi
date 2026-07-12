using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace CodeBrix.JsonPolymorphism.Internal;

internal sealed class DiscriminatorMap
{
    private static readonly ConcurrentDictionary<Type, DiscriminatorMap> Cache = new();

    private DiscriminatorMap(string propertyName, Dictionary<string, Type> knownTypes, Type fallbackType)
    {
        PropertyName = propertyName;
        KnownTypes = knownTypes;
        FallbackType = fallbackType;
    }

    internal string PropertyName { get; }

    internal IReadOnlyDictionary<string, Type> KnownTypes { get; }

    internal Type FallbackType { get; }

    internal static DiscriminatorMap Get(Type baseType) => Cache.GetOrAdd(baseType, Build);

    private static DiscriminatorMap Build(Type baseType)
    {
        var discriminator = baseType.GetCustomAttribute<JsonDiscriminatorAttribute>(inherit: false);

        if (discriminator == null)
        {
            throw new InvalidOperationException(
                $"Type '{baseType}' does not declare [JsonDiscriminator]. Polymorphic deserialization via " +
                "FallbackTypeConverter requires a [JsonDiscriminator(\"<property>\")] attribute on the base " +
                "class or interface.");
        }

        var knownTypes = new Dictionary<string, Type>(StringComparer.Ordinal);

        foreach (var attribute in baseType.GetCustomAttributes<JsonKnownTypeAttribute>(inherit: false))
        {
            ValidateTargetType(baseType, attribute.KnownType, "known");

            if (!knownTypes.TryAdd(attribute.DiscriminatorValue, attribute.KnownType))
            {
                throw new InvalidOperationException(
                    $"Type '{baseType}' declares more than one [JsonKnownType] mapping for discriminator " +
                    $"value '{attribute.DiscriminatorValue}'.");
            }
        }

        var fallbackType = baseType.GetCustomAttribute<JsonFallbackTypeAttribute>(inherit: false)?.FallbackType;

        if (fallbackType != null)
        {
            ValidateTargetType(baseType, fallbackType, "fallback");
        }

        return new DiscriminatorMap(discriminator.PropertyName, knownTypes, fallbackType);
    }

    private static void ValidateTargetType(Type baseType, Type targetType, string role)
    {
        if (targetType == baseType)
        {
            throw new InvalidOperationException(
                $"Type '{baseType}' declares itself as a {role} type, which would recurse endlessly. " +
                "Declare a derived type instead (for example an 'UnknownXyz' subclass).");
        }

        if (!baseType.IsAssignableFrom(targetType))
        {
            throw new InvalidOperationException(
                $"Type '{targetType}' is declared as a {role} type of '{baseType}' but is not assignable to it.");
        }

        var dispatchesFurther = Attribute.IsDefined(targetType, typeof(JsonDiscriminatorAttribute), inherit: false);

        if (!dispatchesFurther && (targetType.IsAbstract || targetType.IsInterface))
        {
            throw new InvalidOperationException(
                $"Type '{targetType}' is declared as a {role} type of '{baseType}' but is not instantiable " +
                "and does not declare its own [JsonDiscriminator] to dispatch further.");
        }
    }
}
