# ExpressionEvaluator
A mathematical expression evaluator with a tokenizer, parser and evaluator.

It supports binary and unary expressions, parentheses, addition, subtraction, multiplication and division.

It incorporates a mechanism for deferring the loading of symbol values from a data source. That means that a general
expression can be processed like "a + b + c" where a, b and c values can be loaded later from any data source.
This functionally resembles the use of variables in an expression.

The parser is NOT a well known algorithm like a recursive descent parser. It is completely an author's implementation 
processing each token sequentially. I cannot tell how much and if so, this results in better performance.

The expression tree that is built by the parser can be "compiled" in the sense that it can be stored.
That way, the evaluation of an expression can skip the parsing phase and therefore result in time gains.

The library includes extensive tests using xUnit.net and Json.NET.
