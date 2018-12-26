using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Linq.Expressions;
using System.Web.Mvc.Html;

namespace ReportWebApp01.Helpers
{
    public static class MyHelper
    {
        public static IHtmlString Mailto(string address, string linktext)
        {
            return MvcHtmlString.Create(
                String.Format("<a href=\"mailto:{0}\">{1}</a>",
                    HttpUtility.HtmlAttributeEncode(address),
                    HttpUtility.HtmlAttributeEncode(linktext)));
        }

        /// <summary>
        /// img タグを作成する。
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="src">イメージのあるパス</param>
        /// <param name="alt">イメージが表示できなかった時の代替テキスト</param>
        /// <returns>img タグ</returns>
        public static IHtmlString Image(this HtmlHelper helper, string src, string alt)
        {
            return MvcHtmlString.Create(
                String.Format("<img src=\"{0}\" alt=\"{1}\" />",
                    HttpUtility.HtmlAttributeEncode(
                        UrlHelper.GenerateContentUrl(src, helper.ViewContext.HttpContext)),
                    HttpUtility.HtmlAttributeEncode(alt)));
        }

        /// <summary>
        /// video タグを作成する。
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="src">ビデオイメージのパス</param>
        /// <param name="htmlAttrs">HTMLオブジェクト</param>
        /// <returns></returns>
        public static IHtmlString Video(this HtmlHelper helper, string src, Object htmlAttrs)
        {
            //video要素を生成
            var builder = new TagBuilder("video");
            // src controls を追加
            builder.MergeAttribute("src",
                UrlHelper.GenerateContentUrl(src, helper.ViewContext.HttpContext));
            builder.MergeAttribute("controls", "controls");
            // その他属性を追加
            builder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttrs));
            // 以上の設定に基づいて、<video>タグを生成
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));
        }


        public static IHtmlString RadioButtonListFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> exp,
            IEnumerable<SelectListItem> list, Object htmlAttrs)
        {
            var sb = new StringBuilder();

            var name = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(
                ExpressionHelper.GetExpressionText(exp));
            var value = ModelMetadata.FromLambdaExpression(exp, helper.ViewData).Model.ToString();

            int i = 1;
            foreach(var item in list)
            {
                var id = String.Format("{0}_{1}", name, i++);

                var label = new TagBuilder("label");
                label.MergeAttributes(
                    HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttrs));

                label.InnerHtml = helper.RadioButton(name, item.Value, (item.Value == value), new { id = id }).ToHtmlString();

                label.InnerHtml += item.Text;
                sb.Append(label.ToString(TagRenderMode.Normal));
            }

            return MvcHtmlString.Create(sb.ToString());
        }
    }
}