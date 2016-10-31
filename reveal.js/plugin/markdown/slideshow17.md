## Object-Oriented Design & Patterns
----

- Object-oriented programming (OOP) is a programming paradigm that uses abstraction to create models based on the real world. OOP uses several techniques from previously established paradigms, including modularity, polymorphism, and encapsulation.

```sh
var Person = function (firstName) {
  this.firstName = firstName;
  console.log('Person instantiated');
};

var person1 = new Person('Alice');
console.log('person1 is ' + person1.firstName); // logs "person1 is Alice"
```