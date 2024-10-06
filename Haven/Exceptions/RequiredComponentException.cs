
namespace Haven.Exceptions;

public class RequiredComponentException : Exception {
	public RequiredComponentException(Type componentType, Type requiredComponentType) : base($"Component {componentType.Name} requires component {requiredComponentType.Name}") { }
}
