
namespace Haven.Ecs.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public class RequireComponentAttribute(params Type[] requiredTypes) : Attribute {
	public Type[] RequiredComponents { get; } = requiredTypes;
}
