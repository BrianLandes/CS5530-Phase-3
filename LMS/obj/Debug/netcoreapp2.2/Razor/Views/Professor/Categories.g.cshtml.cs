#pragma checksum "C:\Users\Brian\Source\Repos\CS5530-Phase-3\LMS\Views\Professor\Categories.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "51e7b1373aa12258af486da4b7d5a8c6ce2e1056"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Professor_Categories), @"mvc.1.0.view", @"/Views/Professor/Categories.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Professor/Categories.cshtml", typeof(AspNetCore.Views_Professor_Categories))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "C:\Users\Brian\Source\Repos\CS5530-Phase-3\LMS\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#line 2 "C:\Users\Brian\Source\Repos\CS5530-Phase-3\LMS\Views\_ViewImports.cshtml"
using LMS;

#line default
#line hidden
#line 3 "C:\Users\Brian\Source\Repos\CS5530-Phase-3\LMS\Views\_ViewImports.cshtml"
using LMS.Models;

#line default
#line hidden
#line 4 "C:\Users\Brian\Source\Repos\CS5530-Phase-3\LMS\Views\_ViewImports.cshtml"
using LMS.Models.AccountViewModels;

#line default
#line hidden
#line 5 "C:\Users\Brian\Source\Repos\CS5530-Phase-3\LMS\Views\_ViewImports.cshtml"
using LMS.Models.ManageViewModels;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"51e7b1373aa12258af486da4b7d5a8c6ce2e1056", @"/Views/Professor/Categories.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"363c4fd446cecdc21217d95f921ea2b5901a3ca3", @"/Views/_ViewImports.cshtml")]
    public class Views_Professor_Categories : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #line hidden
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 2 "C:\Users\Brian\Source\Repos\CS5530-Phase-3\LMS\Views\Professor\Categories.cshtml"
  
    ViewData["Title"] = "Categories";
    Layout = "~/Views/Shared/ProfessorLayout.cshtml";

#line default
#line hidden
            BeginContext(103, 16, true);
            WriteLiteral("\r\n\r\n\r\n\r\n<html>\r\n");
            EndContext();
            BeginContext(119, 936, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("head", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "51e7b1373aa12258af486da4b7d5a8c6ce2e10564153", async() => {
                BeginContext(125, 923, true);
                WriteLiteral(@"
  <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
  <style>
    body {
      font-family: ""Lato"", sans-serif;
    }

    .sidenav {
      /*width: 130px;
      height: 210px;
      position: fixed;
      z-index: 1;
      top: 80px;
      left: 10px;*/
      width: 130px;
      height: 210px;
      position: fixed;
      left: 0;
      right: 0;
      /*margin-left: auto;
      margin-right: auto;*/
      z-index: 1;
      top: 50px;

      background: #eee;
      overflow-x: hidden;
      padding: 8px 0;
    }

      .sidenav a {
        padding: 6px 8px 6px 16px;
        text-decoration: none;
        font-size: 18px;
        color: #2196F3;
        display: block;
      }

        .sidenav a:hover {
          color: #064579;
        }

    .main {
      margin-left: 140px;
      min-height: 200px;
      padding: 0px 10px;
    }
  </style>
");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(1055, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            BeginContext(1057, 2204, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "51e7b1373aa12258af486da4b7d5a8c6ce2e10566265", async() => {
                BeginContext(1063, 35, true);
                WriteLiteral("\r\n\r\n  <div class=\"sidenav\">\r\n    <a");
                EndContext();
                BeginWriteAttribute("href", " href=\'", 1098, "\'", 1221, 8);
                WriteAttributeValue("", 1105, "/Professor/Class?subject=", 1105, 25, true);
#line 62 "C:\Users\Brian\Source\Repos\CS5530-Phase-3\LMS\Views\Professor\Categories.cshtml"
WriteAttributeValue("", 1130, ViewData["subject"], 1130, 20, false);

#line default
#line hidden
                WriteAttributeValue("", 1150, "&num=", 1150, 5, true);
#line 62 "C:\Users\Brian\Source\Repos\CS5530-Phase-3\LMS\Views\Professor\Categories.cshtml"
WriteAttributeValue("", 1155, ViewData["num"], 1155, 16, false);

#line default
#line hidden
                WriteAttributeValue("", 1171, "&season=", 1171, 8, true);
#line 62 "C:\Users\Brian\Source\Repos\CS5530-Phase-3\LMS\Views\Professor\Categories.cshtml"
WriteAttributeValue("", 1179, ViewData["season"], 1179, 19, false);

#line default
#line hidden
                WriteAttributeValue("", 1198, "&year=", 1198, 6, true);
#line 62 "C:\Users\Brian\Source\Repos\CS5530-Phase-3\LMS\Views\Professor\Categories.cshtml"
WriteAttributeValue("", 1204, ViewData["year"], 1204, 17, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(1222, 24, true);
                WriteLiteral(">Assignments</a>\r\n    <a");
                EndContext();
                BeginWriteAttribute("href", " href=\'", 1246, "\'", 1372, 8);
                WriteAttributeValue("", 1253, "/Professor/Students?subject=", 1253, 28, true);
#line 63 "C:\Users\Brian\Source\Repos\CS5530-Phase-3\LMS\Views\Professor\Categories.cshtml"
WriteAttributeValue("", 1281, ViewData["subject"], 1281, 20, false);

#line default
#line hidden
                WriteAttributeValue("", 1301, "&num=", 1301, 5, true);
#line 63 "C:\Users\Brian\Source\Repos\CS5530-Phase-3\LMS\Views\Professor\Categories.cshtml"
WriteAttributeValue("", 1306, ViewData["num"], 1306, 16, false);

#line default
#line hidden
                WriteAttributeValue("", 1322, "&season=", 1322, 8, true);
#line 63 "C:\Users\Brian\Source\Repos\CS5530-Phase-3\LMS\Views\Professor\Categories.cshtml"
WriteAttributeValue("", 1330, ViewData["season"], 1330, 19, false);

#line default
#line hidden
                WriteAttributeValue("", 1349, "&year=", 1349, 6, true);
#line 63 "C:\Users\Brian\Source\Repos\CS5530-Phase-3\LMS\Views\Professor\Categories.cshtml"
WriteAttributeValue("", 1355, ViewData["year"], 1355, 17, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(1373, 21, true);
                WriteLiteral(">Students</a>\r\n    <a");
                EndContext();
                BeginWriteAttribute("href", " href=\'", 1394, "\'", 1522, 8);
                WriteAttributeValue("", 1401, "/Professor/Categories?subject=", 1401, 30, true);
#line 64 "C:\Users\Brian\Source\Repos\CS5530-Phase-3\LMS\Views\Professor\Categories.cshtml"
WriteAttributeValue("", 1431, ViewData["subject"], 1431, 20, false);

#line default
#line hidden
                WriteAttributeValue("", 1451, "&num=", 1451, 5, true);
#line 64 "C:\Users\Brian\Source\Repos\CS5530-Phase-3\LMS\Views\Professor\Categories.cshtml"
WriteAttributeValue("", 1456, ViewData["num"], 1456, 16, false);

#line default
#line hidden
                WriteAttributeValue("", 1472, "&season=", 1472, 8, true);
#line 64 "C:\Users\Brian\Source\Repos\CS5530-Phase-3\LMS\Views\Professor\Categories.cshtml"
WriteAttributeValue("", 1480, ViewData["season"], 1480, 19, false);

#line default
#line hidden
                WriteAttributeValue("", 1499, "&year=", 1499, 6, true);
#line 64 "C:\Users\Brian\Source\Repos\CS5530-Phase-3\LMS\Views\Professor\Categories.cshtml"
WriteAttributeValue("", 1505, ViewData["year"], 1505, 17, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(1523, 1731, true);
                WriteLiteral(@">Assignment Categories</a>
  </div>


  <div class=""main"">
    <h4 id=""classname"">Class</h4>

    <div id=""departmentDiv"" class=""col-md-12"">
    <div class=""panel panel-primary"">
      <div class=""panel-heading"">
        <h3 class=""panel-title""></h3>
      </div>
      <div class=""panel-body"">
        <table id=""tblCategories"" class=""table table-bordered table-striped table-responsive table-hover"">
          <thead>
            <tr>
              <th align=""left"" class=""productth"">Name</th>
              <th align=""left"" class=""productth"">Weight</th>
            </tr>
          </thead>
          <tbody></tbody>
        </table>
      </div>
    </div>
  </div>


    <div class=""col-md-12"">
      <div class=""panel panel-primary"">
        <div class=""panel-heading"">
          <h3 class=""panel-title"">New Category</h3>
        </div>
        <div class=""panel-body"">
          <div class=""form-group col-md-5"">
            <label>Category Name</label>
            <input type=""tex");
                WriteLiteral(@"t"" name=""CatName"" id=""CatName"" class=""form-control"" placeholder=""Enter category name"" required="""" />
          </div>
          <div class=""form-group col-md-5"">
            <label>Category Weight</label>
            <input type=""text"" name=""CategoryWeight"" id=""CategoryWeight"" class=""form-control"" placeholder=""Enter category weight"" required="""" />
          </div>

          <div class=""form-group col-md-1"">
            <div style=""float: right; display:inline-block;"">
              <input class=""btn btn-primary"" name=""submitButton"" id=""btnSave"" value=""Add"" type=""button"" onclick=""AddCategory()"">
            </div>
          </div>
        </div>
      </div>
    </div>


  </div>
");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(3261, 21, true);
            WriteLiteral("\r\n</html>\r\n\r\n\r\n\r\n\r\n\r\n");
            EndContext();
            DefineSection("Scripts", async() => {
                BeginContext(3303, 246, true);
                WriteLiteral("\r\n  <script type=\"text/javascript\">\r\n\r\n    LoadData();\r\n\r\n    function AddCategory() {\r\n\r\n      var catName = $(\"#CatName\").val();\r\n      var catWeight = Number($(\"#CategoryWeight\").val());\r\n\r\n      $.ajax({\r\n        type: \'POST\',\r\n        url: \'");
                EndContext();
                BeginContext(3550, 51, false);
#line 137 "C:\Users\Brian\Source\Repos\CS5530-Phase-3\LMS\Views\Professor\Categories.cshtml"
         Write(Url.Action("CreateAssignmentCategory", "Professor"));

#line default
#line hidden
                EndContext();
                BeginContext(3601, 68, true);
                WriteLiteral("\',\r\n        dataType: \'json\',\r\n        data: {\r\n          subject: \'");
                EndContext();
                BeginContext(3670, 19, false);
#line 140 "C:\Users\Brian\Source\Repos\CS5530-Phase-3\LMS\Views\Professor\Categories.cshtml"
               Write(ViewData["subject"]);

#line default
#line hidden
                EndContext();
                BeginContext(3689, 27, true);
                WriteLiteral("\',\r\n          num: Number(\'");
                EndContext();
                BeginContext(3717, 15, false);
#line 141 "C:\Users\Brian\Source\Repos\CS5530-Phase-3\LMS\Views\Professor\Categories.cshtml"
                  Write(ViewData["num"]);

#line default
#line hidden
                EndContext();
                BeginContext(3732, 24, true);
                WriteLiteral("\'),\r\n          season: \'");
                EndContext();
                BeginContext(3757, 18, false);
#line 142 "C:\Users\Brian\Source\Repos\CS5530-Phase-3\LMS\Views\Professor\Categories.cshtml"
              Write(ViewData["season"]);

#line default
#line hidden
                EndContext();
                BeginContext(3775, 28, true);
                WriteLiteral("\',\r\n          year: Number(\'");
                EndContext();
                BeginContext(3804, 16, false);
#line 143 "C:\Users\Brian\Source\Repos\CS5530-Phase-3\LMS\Views\Professor\Categories.cshtml"
                   Write(ViewData["year"]);

#line default
#line hidden
                EndContext();
                BeginContext(3820, 923, true);
                WriteLiteral(@"'),
          category: catName,
          catweight: catWeight},
        success: function (data, status) {
          //alert(JSON.stringify(data));
          if (!data.success) {
            alert(""Unable to add category"");
          }
          LoadData();


        },
        error: function (ex) {
          var r = jQuery.parseJSON(response.responseText);
          alert(""Message: "" + r.Message);
          alert(""StackTrace: "" + r.StackTrace);
          alert(""ExceptionType: "" + r.ExceptionType);
        }
        });

    }

    function PopulateTable(tbl, offerings) {
      var newBody = document.createElement(""tbody"");

      $.each(offerings, function (i, item) {
        var tr = document.createElement(""tr"");

        var td = document.createElement(""td"");
        var a = document.createElement(""a"");
        a.setAttribute(""href"", ""/Professor/CatAssignments/?subject="" + '");
                EndContext();
                BeginContext(4744, 19, false);
#line 173 "C:\Users\Brian\Source\Repos\CS5530-Phase-3\LMS\Views\Professor\Categories.cshtml"
                                                                   Write(ViewData["subject"]);

#line default
#line hidden
                EndContext();
                BeginContext(4763, 15, true);
                WriteLiteral("\' + \"&num=\" + \'");
                EndContext();
                BeginContext(4779, 15, false);
#line 173 "C:\Users\Brian\Source\Repos\CS5530-Phase-3\LMS\Views\Professor\Categories.cshtml"
                                                                                                      Write(ViewData["num"]);

#line default
#line hidden
                EndContext();
                BeginContext(4794, 18, true);
                WriteLiteral("\' + \"&season=\" + \'");
                EndContext();
                BeginContext(4813, 18, false);
#line 173 "C:\Users\Brian\Source\Repos\CS5530-Phase-3\LMS\Views\Professor\Categories.cshtml"
                                                                                                                                        Write(ViewData["season"]);

#line default
#line hidden
                EndContext();
                BeginContext(4831, 16, true);
                WriteLiteral("\' + \"&year=\" + \'");
                EndContext();
                BeginContext(4848, 16, false);
#line 173 "C:\Users\Brian\Source\Repos\CS5530-Phase-3\LMS\Views\Professor\Categories.cshtml"
                                                                                                                                                                           Write(ViewData["year"]);

#line default
#line hidden
                EndContext();
                BeginContext(4864, 441, true);
                WriteLiteral(@"' + ""&cat="" + item.name);
        a.appendChild(document.createTextNode(item.name));
        td.appendChild(a);
        tr.appendChild(td);

        var td = document.createElement(""td"");
        td.appendChild(document.createTextNode(item.weight));
        tr.appendChild(td);

        newBody.appendChild(tr);
      });

      tbl.appendChild(newBody);

    }

    function LoadData() {



      classname.innerText = '");
                EndContext();
                BeginContext(5306, 19, false);
#line 193 "C:\Users\Brian\Source\Repos\CS5530-Phase-3\LMS\Views\Professor\Categories.cshtml"
                        Write(ViewData["subject"]);

#line default
#line hidden
                EndContext();
                BeginContext(5325, 1, true);
                WriteLiteral(" ");
                EndContext();
                BeginContext(5327, 15, false);
#line 193 "C:\Users\Brian\Source\Repos\CS5530-Phase-3\LMS\Views\Professor\Categories.cshtml"
                                             Write(ViewData["num"]);

#line default
#line hidden
                EndContext();
                BeginContext(5342, 1, true);
                WriteLiteral(" ");
                EndContext();
                BeginContext(5344, 18, false);
#line 193 "C:\Users\Brian\Source\Repos\CS5530-Phase-3\LMS\Views\Professor\Categories.cshtml"
                                                              Write(ViewData["season"]);

#line default
#line hidden
                EndContext();
                BeginContext(5362, 1, true);
                WriteLiteral(" ");
                EndContext();
                BeginContext(5364, 16, false);
#line 193 "C:\Users\Brian\Source\Repos\CS5530-Phase-3\LMS\Views\Professor\Categories.cshtml"
                                                                                  Write(ViewData["year"]);

#line default
#line hidden
                EndContext();
                BeginContext(5380, 206, true);
                WriteLiteral("\';\r\n\r\n      var tbl = document.getElementById(\"tblCategories\");\r\n      var body = tbl.getElementsByTagName(\"tbody\")[0];\r\n      tbl.removeChild(body);\r\n\r\n      $.ajax({\r\n        type: \'POST\',\r\n        url: \'");
                EndContext();
                BeginContext(5587, 50, false);
#line 201 "C:\Users\Brian\Source\Repos\CS5530-Phase-3\LMS\Views\Professor\Categories.cshtml"
         Write(Url.Action("GetAssignmentCategories", "Professor"));

#line default
#line hidden
                EndContext();
                BeginContext(5637, 68, true);
                WriteLiteral("\',\r\n        dataType: \'json\',\r\n        data: {\r\n          subject: \'");
                EndContext();
                BeginContext(5706, 19, false);
#line 204 "C:\Users\Brian\Source\Repos\CS5530-Phase-3\LMS\Views\Professor\Categories.cshtml"
               Write(ViewData["subject"]);

#line default
#line hidden
                EndContext();
                BeginContext(5725, 27, true);
                WriteLiteral("\',\r\n          num: Number(\'");
                EndContext();
                BeginContext(5753, 15, false);
#line 205 "C:\Users\Brian\Source\Repos\CS5530-Phase-3\LMS\Views\Professor\Categories.cshtml"
                  Write(ViewData["num"]);

#line default
#line hidden
                EndContext();
                BeginContext(5768, 24, true);
                WriteLiteral("\'),\r\n          season: \'");
                EndContext();
                BeginContext(5793, 18, false);
#line 206 "C:\Users\Brian\Source\Repos\CS5530-Phase-3\LMS\Views\Professor\Categories.cshtml"
              Write(ViewData["season"]);

#line default
#line hidden
                EndContext();
                BeginContext(5811, 28, true);
                WriteLiteral("\',\r\n          year: Number(\'");
                EndContext();
                BeginContext(5840, 16, false);
#line 207 "C:\Users\Brian\Source\Repos\CS5530-Phase-3\LMS\Views\Professor\Categories.cshtml"
                   Write(ViewData["year"]);

#line default
#line hidden
                EndContext();
                BeginContext(5856, 440, true);
                WriteLiteral(@"')},
        success: function (data, status) {
          //alert(JSON.stringify(data));
          PopulateTable(tbl, data);
        },
        error: function (ex) {
          var r = jQuery.parseJSON(response.responseText);
          alert(""Message: "" + r.Message);
          alert(""StackTrace: "" + r.StackTrace);
          alert(""ExceptionType: "" + r.ExceptionType);
        }
        });

        
    }

  </script>

");
                EndContext();
            }
            );
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
