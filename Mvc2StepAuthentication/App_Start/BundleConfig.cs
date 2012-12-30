using System.Web.Optimization;

namespace Mvc2StepAuthentication
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery*"));

            bundles.Add(new StyleBundle("~/bundles/styles").Include(
                "~/Styles/site.css"));
        }
    }
}