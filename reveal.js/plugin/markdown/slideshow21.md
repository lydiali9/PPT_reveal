## JSON
----

- JSON: The JavaScript Object Notation (JSON) is a data-interchange format.  Although not a strict subset, JSON closely resembles a subset of JavaScript syntax. Though many programming languages support JSON, JSON is especially useful for JavaScript-based apps, including websites and browser extensions.

```sh
var code = '"\u2028\u2029"';
JSON.parse(code); // works fine
eval(code); // fails
```