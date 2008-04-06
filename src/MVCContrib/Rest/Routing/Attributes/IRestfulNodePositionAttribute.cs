namespace MvcContrib.Rest.Routing.Attributes
{
	/// <summary>Attribute has a <see cref="NodePosition"/>.</summary>
	public interface IRestfulNodePositionAttribute
	{
		/// <summary>The position in the attribute should appear as if you were manually building the route.</summary>
		/// <example>
		/// <para>The two code blocks below show an equivelient means of creating the same route. 
		/// The first uses the builder and the second uses attributes. The <see cref="NodePosition"/> is used to ensure when we 
		/// reflect and discover the attributes that they are applied in the correct order so we get the same
		/// output as we would should we have used the builder manually.</para>
		/// 
		/// <code lang="C#">routeBuilder.ForController&lt;FooController&gt;()
		///   .ToRequiredParameter(&quot;type&quot;, new string[]{&quot;foo&quot;,&quot;bar&quot;})
		///   .ToController()
		///   .ToOptionalParameter(&quot;isCrazy&quot;, new Hash(@true=>&quot;is-crazy&quot;,@false=>&quot;is-not-crazy&quot;))
		///  .Register();
		/// </code>
		/// <para>Here is the attribute usage for the equivelent builder code from above.</para>
		/// <code lang="C#">
		/// [SimplyRestfulRoute, 
		///  RequiredString(NodePosition=1, Name=&quot;type&quot;, AcceptedValues=new string[]{&quot;foo&quot;,&quot;bar&quot;}),
		///  ToController(NodePosition=2),
		///  OptionalString(NodePosition=3, Name=&quot;isCrazy&quot;, AcceptedValues=new string[]{&quot;true=is-crazy&quot;, &quot;false=is-not-crazy&quot;})]
		/// public class FooController
		/// {
		/// }
		/// </code>
		/// </example>
		int NodePosition { get; set; }
	}
}