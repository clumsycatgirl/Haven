
namespace Haven.Diagnostics;

public static class ImmutabilityChecker {
	private static List<string> exceptions = new();

	public static void CheckImmutability(Type type) {
		if (type.GetCustomAttribute<ImmutableAttribute>() is null)
			return;

		IEnumerable<FieldInfo> nonReadOnlyFields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
			.Where(field => !field.IsInitOnly);

		if (nonReadOnlyFields.Any())
			exceptions.Add($"Class {type.Name} marked as immutable has non-readonly fields: {string.Join(", ", nonReadOnlyFields.Select(f => f.Name))}");

		IEnumerable<PropertyInfo> mutableProperties = type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
			.Where(property => property.CanWrite);

		if (mutableProperties.Any())
			exceptions.Add($"Class {type.Name} marked as immutable has mutable properties: {string.Join(", ", mutableProperties.Select(p => p.Name))}");
	}

	static ImmutabilityChecker() {
		IEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies();

		foreach (Assembly assembly in assemblies) {
			IEnumerable<Type> typesToCheck = assembly.GetTypes()
				.Where(type => type.GetCustomAttribute<ImmutableAttribute>() is not null);

			foreach (Type type in typesToCheck) {
				CheckImmutability(type);
			}

		}

		if (exceptions.Any()) {
			foreach (string exception in exceptions) {
				Console.WriteLine(exception);
			}

			System.Environment.Exit(1);
		}
	}

	public static void Initialize() { }
}
