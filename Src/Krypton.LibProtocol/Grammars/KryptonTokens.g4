lexer grammar KryptonTokens;

IMPORT : 'import' ;
GROUP : 'group' ;
LIBRARY : 'library' ;
PROTOCOL : 'protocol' ;
PACKET : 'packet' ;
DECLARE : 'declare' ;
META : 'meta' ;
KPDL : 'kpdl' ;

ENUMERABLE : 'enumerable' ;

PRIMITIVE 
  : BYTE 
  | BOOL
  | INT8 | UINT8 
  | INT16 | UINT16
  | INT32 | UINT32
  | INT64 | UINT64
  | STRING | CSTRING
  | BUFFER
  ;

fragment BYTE : 'byte' ;
fragment BOOL : 'bool' ;
fragment INT8 : 'int8' ;
fragment UINT8 : 'uint8' ;
fragment INT16 : 'int16' ;
fragment UINT16 : 'uint16' ;
fragment INT32 : 'int32' ;
fragment UINT32 : 'uint32' ;
fragment INT64 : 'int64' ;
fragment UINT64 : 'uint64' ;
fragment STRING : 'string' ;
fragment CSTRING : 'cstring' ;
fragment BUFFER : 'buffer';

TRUE : 'true' ;
FALSE : 'false' ;

GREATER : '>' ;
LESS : '<' ;
fragment EQUAL : '==' ;
fragment NOTEQUAL : '!=' ;
fragment GREATER_OR_EQUAL : '>=' ;
fragment LESS_OR_EQUAL : '<=' ;

OPERATOR 
  : EQUAL | NOTEQUAL
  | GREATER_OR_EQUAL | GREATER
  | LESS_OR_EQUAL | LESS
  ;

DIRECTIVE : '=>' ;
PERIOD : '.' ;
COMMA : ',' ;
COLON : ':' ;
DOUBLECOLON : '::' ;
SEMICOLON : ';' ;
LSBRACKET : '[' ;
RSBRACKET : ']' ;
LBRACKET : '{' ;
RBRACKET : '}' ;
LPAREN : '(' ;
RPAREN : ')' ;
FSLASH : '/' ;
LOOPOPERATOR : '..' ;
fragment PLUSPLUS : '++' ;

METAOPERATOR
    : PLUSPLUS
    ;

IDENTIFIER 
    : [A-Za-z_][A-Za-z_0-9]+ 
    ;

INTEGER : [0-9]+ ;

// skip whitespace
WS : [ \t\r\n]+ -> skip ;
