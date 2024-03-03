
namespace Haven.Ecs.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class DependsOnAttribute(params Type[] dependentSystems) : Attribute {
	public Type[] DependentSystems { get; } = dependentSystems;
}
