namespace MvcContrib.TestHelper
{
  /// <summary>
  /// The data created by a controller calling RedirectToAction
  /// </summary>
    public class RedirectToActionData
    {
      /// <summary>
      /// Gets or sets the name of the action passed to RedirectToAction in the controller
      /// </summary>
      /// <value>The name of the action.</value>
        public string ActionName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the controller passed to RedirectToAction in the controller
        /// </summary>
        /// <value>The name of the controller.</value>
        public string ControllerName
        {
            get;
            set;
        }
    }
}
