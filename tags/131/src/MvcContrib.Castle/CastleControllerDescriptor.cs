using MvcContrib.MetaData;

namespace MvcContrib.Castle
{
	///<summary>
	/// Controller Descriptor that uses the CastleSimpleBinder for default binding.
	///</summary>
	public class CastleControllerDescriptor : ControllerDescriptor
	{
		protected override IParameterBinder GetParameterBinder(ActionParameterMetaData parameterMetaData) {
			IParameterBinder binder = base.GetParameterBinder(parameterMetaData);
			if(binder as SimpleParameterBinder != null)
			{
				return new CastleSimpleBinder();
			}
			return binder;
		}
	}
}