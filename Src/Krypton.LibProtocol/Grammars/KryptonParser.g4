parser grammar KryptonParser;

options 
    { tokenVocab=KryptonTokens; }

init : groups+=group_definition* libraries+=library_definition* protocols+=protocol_definition*;

group_definition : GROUP name=IDENTIFIER ';' ;

library_definition : LIBRARY name=IDENTIFIER '{' packets+=packet_definition* '}' ;

protocol_definition : PROTOCOL 
                      ns=namespace '[' name=IDENTIFIER ']'
                      '{' statements=proto_statements '}' ;

proto_statements : messages+=message_definitions packets+=packet_definition* ;

message_definitions : name=IDENTIFIER (',' message_definitions)?
                    ;

packet_definition : PACKET name=IDENTIFIER (':' parents+=packet_parents)? 
                    '{' statements+=packet_statement+ '}' ;

packet_parents : ns=namespace '[' name=IDENTIFIER ']' (',' packet_parents)?
               ;

packet_statement : datadef=data_definition
                 | condef=conditional_definition
                 ;

data_definition : type=PRIMITIVE name=IDENTIFIER ';' ; 
conditional_definition : '(' condition ')' '=>' 
                         '{' statements+=packet_statement+ '}' ';' ;
condition : val1=condition_value op=OPERATOR val2=condition_value ;

condition_value : TRUE | FALSE | INTEGER | IDENTIFIER ;
namespace : IDENTIFIER ('.' IDENTIFIER)* ;
