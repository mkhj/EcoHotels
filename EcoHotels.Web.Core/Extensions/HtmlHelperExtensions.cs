using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using EcoHotels.Core.Common.Enum;
using EcoHotels.Web.Core.Models;


namespace EcoHotels.Web.Core.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString LanguageSelectorFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, int selectedLanguageId)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            var localizedValues = (List<LocalizedValueModel>)metadata.Model;
            var expressionText = ExpressionHelper.GetExpressionText(expression);

            var result = new StringBuilder();

            var group = new TagBuilder("div");
            group.AddCssClass("btn-group");

            if (localizedValues.Count > 1)
            {
                for (var i = 0; i < localizedValues.Count; i++)
                {
                    var aTag = new TagBuilder("a");
                    aTag.AddCssClass(localizedValues[i].Id == selectedLanguageId ? "btn active" : "btn");

                    var iTag = new TagBuilder("i");
                    iTag.Attributes.Add("hreflang", localizedValues[i].LanguageName);

                    aTag.InnerHtml = iTag.ToString();

                    result.Append(aTag.ToString());
                }
            }

            group.InnerHtml = result.ToString();

            return MvcHtmlString.Create(group.ToString());

            //var result = new StringBuilder();

            //if(localizedValues.Count > 1)
            //{
            //    for (var i = 0; i < localizedValues.Count; i++)
            //    {
            //        var aTag = new TagBuilder("a");

            //        aTag.AddCssClass(localizedValues[i].Id == selectedLanguageId ? "selected" : string.Empty);
            //        aTag.Attributes.Add("hreflang", localizedValues[i].LanguageName);

            //        result.Append(aTag.ToString());
            //    }
            //}

            //var divTag = new TagBuilder("div");
            //divTag.Attributes.Add("id", "language-selector");

            //divTag.InnerHtml = result.ToString();

            //return MvcHtmlString.Create(divTag.ToString());
        }

        public static MvcHtmlString TextBoxLocalizationFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, LanguageTypeEnum selectedLanguageId, object htmlAttributes)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            var localizedValues = (List<LocalizedValueModel>)metadata.Model;
            var expressionText = ExpressionHelper.GetExpressionText(expression);

            var result = new StringBuilder();
           
            for (var i = 0; i < localizedValues.Count; i++)
            {
                var pTag = new TagBuilder("span");
                {
                    pTag.AddCssClass("localized");
                    pTag.AddCssClass(localizedValues[i].Id == (int) selectedLanguageId ? string.Empty : "hidden");

                    // Hidden Id
                    var id = string.Format("{0}[{1}].Id", expressionText, i);
                    pTag.InnerHtml += helper.Hidden(id, localizedValues[i].Id).ToHtmlString();

                    // Textbox
                    var name = string.Format("{0}[{1}].Value", expressionText, i);
                    pTag.InnerHtml += helper.TextBox(name, localizedValues[i].Value, htmlAttributes).ToHtmlString();

                    if(localizedValues.Count > 1)
                    {
                        // Flag
                        var aTag = new TagBuilder("i");
                        aTag.Attributes.Add("hreflang", localizedValues[i].LanguageName);

                        pTag.InnerHtml += aTag.ToString();                        
                    }
                }

                result.Append(pTag.ToString());
            }

            return MvcHtmlString.Create(result.ToString());
        }

        public static MvcHtmlString TextAreaLocalizationFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, LanguageTypeEnum selectedLanguageId, object htmlAttributes)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            var localizedValues = (List<LocalizedValueModel>)metadata.Model;
            var expressionText = ExpressionHelper.GetExpressionText(expression);

            var result = new StringBuilder();

            for (var i = 0; i < localizedValues.Count; i++)
            {
                var pTag = new TagBuilder("span");
                {
                    pTag.AddCssClass("localized");
                    pTag.AddCssClass(localizedValues[i].Id == (int) selectedLanguageId ? string.Empty : "hidden");

                    // Hidden Id
                    var id = string.Format("{0}[{1}].Id", expressionText, i);
                    pTag.InnerHtml += helper.Hidden(id, localizedValues[i].Id).ToHtmlString();

                    // Textbox
                    var name = string.Format("{0}[{1}].Value", expressionText, i);
                    pTag.InnerHtml += helper.TextArea(name, localizedValues[i].Value, htmlAttributes).ToHtmlString();

                    if (localizedValues.Count > 1)
                    {
                        // Flag
                        var aTag = new TagBuilder("i");
                        aTag.Attributes.Add("hreflang", localizedValues[i].LanguageName);

                        pTag.InnerHtml += aTag.ToString();
                    }
                }

                result.Append(pTag.ToString());
            }

            return MvcHtmlString.Create(result.ToString());
        }
    }
}