## Ajax($.ajax、$.post、$.get)
----

- Asynchronous JavaScript + XML, while not a technology in itself, is a term coined in 2005 by Jesse James Garrett, that describes a "new" approach to using a number of existing technologies together, including: HTML or XHTML, Cascading Style Sheets, JavaScript, The Document Object Model, XML, XSLT, and most importantly the XMLHttpRequest object.

```sh
$.ajax({
    type: 'POST',
    url: url,
    data: data,
    dataType: dataType,
    success: success
});
```