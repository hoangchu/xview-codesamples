# XView Code Samples
This project provides code samples to illustrate the usage of XView.

XView project can be found here: [http://xview.codeplex.com](http://xview.codeplex.com).

# XView Basic Concept
Before diving into XView coding it's recommended to get to know the basic concept of XView.

## Decoupled And Clean
XView comes with a tiny XView.dll (less than 40Kb) and has a very small and clean footprint in Tridion, thanks to its logical and practical decoupled design.

Here are some of the characteristics of a TOM.NET templates project using XView:

- A C# class library that has a reference to the XView.dll (beside some Tridion dlls).
- That C# class library produces a DLL and a Template Building Block (TBB).
- That TBB is an XView Controller and it's used in all Compound Templates in Tridion.
- Each Compound Template has one single TBB: the XView Controller.
- The XView Controller produces one package variable: Output.
- Some happy faces.

As many can agree that developing .NET class libraries can be one of the most satisfying coding experiences, it is fair to say that it's a joy to code TOM.NET templates class libraries using XView. More so, if you've experienced otherwise (i.e. without XView).

The picture below depicts the XView's decoupled architecture.
![XView Architecture](xview.png)

## View 
Tridion templating essentially involves developing Page Templates and Component Templates to respectively render Pages and Components,.

In XView **View** classes are mapped to **Page Templates** and **Component Templates**. For each Template in Tridion there is a View class in an XView templates class library. It is possible to map two or more Templates to a single View class.

See section ViewMapper bellow for mapping details.

[Example View class](https://github.com/hoangchu/xview-codesamples/blob/master/Source/Templating/Views/ComponentViews/ArticleView.cs).

## Model

By default a Tridion **Page** object or a **Component** object is injected into a View as a **Model**. Model is a strongly typed property of a View class. The Model of a View that's mapped to a Page Template has the type Page. The Model of a View that's mapped to a Component Template has the type Component.

## Context
The rendition of a Component or Page always take place inside a **Publication**. This Publication is part of a **Blueprint**. (Be it a single Publication Blueprint). A Blueprint exists in a **Tridion Content Manager instance** which runs inside an **environment** (as in DTAP).

Common business rules and high-level content structures are normally defined on Publication and Blueprint level. For example, in a multilingual website implementation it is common to have a country, a language and a publication configuration. Publication configuration, for example, is defined on Blueprint level (parent Publication) and localized in each child Publication.  

In XView the **TridionContext** class embodies the context in which a Page or a Component rendition takes place. The TridionContext **encapsulates** and **exposes** the **common business rules**, **high-level content structures** and **configurations**, and **environment data**.

The TridionContext object is loaded and injected into a View to make a View "context aware".

The default TridionContext class contains properties and methods that are applicable to all Tridion implementations. XView allows to extend the TridionContext. It should be a common thing to extend the TridionContext to implement project specific business rules and configurations.

[Example TridionContext derived class](https://github.com/hoangchu/xview-codesamples/blob/master/Source/Templating/Controllers/IntranetContext.cs).

## Controller
Templates in Tridion are called Compound Templates. Each Compound Template can contain one or more Template Building Blocks (TBB). A TBB contains logic to render output for a Page or a Component. The TBBs in a Compound Template get executed in the order in which they're added.

In XView each **Compound Template** has **one TBB**. The same TBB is used on all Compound Templates inside the same Blueprint. (The Context). This TBB is the **Controller** in XView.

The Controller in XView is a [Front Controller](https://en.wikipedia.org/wiki/Front_Controller_pattern). It interfaces with Tridion via the ITemplate interface. The Controller is responsible for tasks including the following.

- Decoupling Tridion (to allow development and maintenance of template logic and layouts outside of Tridion).
- Controlling the execution flow of a rendition.
- Loading and injecting TridionContext into Views.
- Dispatching render tasks to Views.

In XView the single thing that is mandatory is to create a `Controller` derived class inside the `[Project root namespace].Controllers` namespace. This derived Controller is the TBB used on all Templates inside the same Blueprint.

[Example Controller derived class](https://github.com/hoangchu/xview-codesamples/blob/master/Source/Templating/Controllers/IntranetController.cs).

## ViewMapper
By default Page Templates are mapped to View classes inside the PageViews subnamespace while Component Templates are mapped to View classes inside ComponentViews subnamespace. The mapping of Templates to Views is done automatically by the default ViewMapper class. 

The default mapping logic is based on Template and View naming convention.

Examples:

- A Component Template named `Article` is mapped with a View named `ArticleView`. This View's full name is `[Project root namespace].Views.ComponentViews.ArticleView`.

- A Page Template named `Xml` is mapped with a View named `XmlView`. This View's full name is `[Project root namespace].Views.PageViews.XmlView`.

It is possible to provide custom mapping logic by creating a ViewMapper derived class and override a specific method, or by creating a new class that implements the IViewMapper interface.

[Example ViewMapper derived class](https://github.com/hoangchu/xview-codesamples/blob/master/Source/Templating/Controllers/IntranetViewMapper.cs).

## Output Filters
Output decoration and output validation are among the things you often see in a Tridion templating implementation. 

XView provides a convenient and centralized way to decorate output with **OutputDecorationFilter**s and validate output with **OutputValidationFilter**s.

You can create filter classes derived from `OutputDecorationFilter` or `OutputValidationFilter` and specify which `ViewOutputType` to apply the filtering to. (Eg. Html, Xml, Css, Json, etc.). A filter class can handle one or more `ViewOutputType`. The filter will be applied to output of `View`'s that have a matching `ViewOutputType`. (In a `View` class you can explicitly specify a `ViewOutputType` through the `View.OutputType` property. By default `ViewOutputType.Html` is returned for `View.OutputType` when not explicitly specified).

OutputDecorationFilter and OutputValidationFilter derived classes can be registered inside the Controller to perform output filtering. 

[Example of an OutputDecorationFilter class](https://github.com/hoangchu/xview-codesamples/blob/master/Source/Templating/DecorationFilters/DefaultFinishActionsDecorationFilter.cs).

[Example of an OutputValidationFilter class](https://github.com/hoangchu/xview-codesamples/blob/master/Source/Templating/ValidationFilters/XmlValidationFilter.cs).

[Example of how to register OutputDecorationFilter or OutputValidationFilter derived types inside a Controller](https://github.com/hoangchu/xview-codesamples/blob/master/Source/Templating/Controllers/IntranetController.cs).

## XTemplate: Clean C# and HTML
Generating HTML output is one of the main things that's done with Tridion templates development. 

In XView HTML output can be generated cleanly inside a View class by loading and parsing a separate HTML template using XTemplate.

HTML templates are added as .html embedded resource files to a class library. Visual Studio provides a convenient way to expose/access embedded resource via a resource XML file (resx).

The syntax used in HTML template and interpreted by XTemplate is simple. There are only three tags you need to learn; a variable tag, a block begin and a block end tag.

### Variable
Variables are placeholder tags defined in HTML templates. Each variable tag consists of an opening curly bracket, followed by a name, then ends with a closing curly bracket. Values can be assigned to these variable tags using XTemplate.

Here are some example variable tags:

- `{Title}`
- `{Introduction}`
- `{VariableName}`

### Block
A block is an HTML snippet that's enclosed by an opening block tag and a closing block tag. Each block has a name. The name of a block is specified in both the opening and the closing block tags.

For example, here's the opening block tag for a block named "paragraph":

```html
<!-- BEGIN: paragraph -->
```

And here's the closing block tag for block "paragraph":

```html
<!-- END: paragraph -->
```

Using XTemplate you can decide whether to output a block (once or multiple times) or to omit it based on some conditions. 

For example in an iteration you want to output the same HTML block multiple times, but each with different content. Or you decide not to output (omit) a "related-content" block, because there is no related content available.

Bellow is an example of an HTML template and a C# View code snippet that parses the HTML template using XTemplate.

HTML template
```html
...

<!-- BEGIN: anchors -->
<ul>
	<!-- BEGIN: anchor -->
	<li><a href="#{ParagraphIndex}">{ParagraphTitle}</a></li>
	<!-- END: anchor -->
</ul>
<!-- END: anchors -->

...
```

C# code
```csharp
dynamic xt = new XTemplate(Layout.Article);

...

IList<ItemFields> paragraphs = fields.GetEmbeddedFields("paragraphs");
int paragraphIndex = 0;

foreach (ItemFields paragraph in paragraphs)
{
	paragraphIndex++;
    
	xt.ParagraphIndex = paragraphIndex;
	xt.ParagraphTitle = paragraph.GetText("title);
	xt.Parse("root.anchors.anchor");
}

xt.Parse("root.anchors");

...

return xt.ToString();
```

XTemplate is based on this nice little [PHP XTemplate class](http://www.phpxtemplate.org/). Relevant features are rewritten and new features are added to take advantage of .NET/C#.

[Example C# View class](https://github.com/hoangchu/xview-codesamples/blob/master/Source/Templating/Views/ComponentViews/ArticleView.cs)

[Example HTML template](https://github.com/hoangchu/xview-codesamples/blob/master/Source/Templating/Views/ComponentViews/Layout/ArticleView.html)
