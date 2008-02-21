namespace MvcContrib.TestHelper
{
  /// <summary>
  /// The data created by a controller calling RenderView
  /// </summary>
    public class RenderViewData
    {
      /// <summary>
      /// Gets or sets the name of the view passed to RenderView in the controller
      /// </summary>
      /// <value>The name of the view.</value>
        public string ViewName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the master page passed to RenderView in the controller
        /// </summary>
        /// <value>The name of the master page.</value>
        public string MasterName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the ViewData passed to RenderView in the controller.
        /// </summary>
        /// <value>The ViewData.</value>
        public object ViewData
        {
            get;
            set;
        }
    }
}
