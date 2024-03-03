
namespace Haven.Ecs.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public class RequireComponent(params Type[] requiredTypes) : Attribute {
	public Type[] RequiredTypes { get; } = requiredTypes;
}

public static class RequireComponentValidator {
	public static void Validate(object obj) {
		Type type = obj.GetType();

		foreach (PropertyInfo property in type.GetProperties()) {
			RequireComponent attribute = (RequireComponent)Attribute.GetCustomAttribute(property, typeof(RequireComponent));

			if (attribute is not null) {
				IEnumerable<object> list = (IEnumerable<object>)property.GetValue(obj);

				foreach (Type requiredType in attribute.RequiredTypes) {
					if (!list.Any(item => item.GetType() == requiredType)) {
						throw new InvalidOperationException($"{type.Name} must contain an instance of {requiredType.Name} in {property.Name}");
					}
				}
			}
		}
	}
}
