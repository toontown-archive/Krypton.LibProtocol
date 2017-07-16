parser grammar KryptonParser;

options 
    { tokenVocab=KryptonTokens; }

// Root members

init : imports+=import_statement* groups+=group_definition* libraries+=library_definition* protocols+=protocol_definition*
     ;

import_statement : IMPORT (path=directory)? file=IDENTIFIER '.' KPDL ';'
                 ;

group_definition : GROUP name=IDENTIFIER ';' ;

protocol_definition : PROTOCOL 
                      ns=namespace '[' name=IDENTIFIER ']'
                      '{' message_definitions? packet_definition* '}' ;

message_definitions : name=IDENTIFIER (',' message_definitions)?
                    ;

packet_definition : PACKET name=IDENTIFIER (':' packet_parent)? 
                    '{' operation_statement+ '}' ;

packet_parent : ns=namespace '[' name=IDENTIFIER ']' (',' packet_parent)?
              ;

library_definition : LIBRARY name=IDENTIFIER '{' member_options? library_statement* '}' ;

library_statement
    : packet_definition
    | type_declaration
    ;

// Types

type_reference 
    : builtin_type_reference
    | declared_type_reference
    | local_type_reference
    | generic_attribute_reference
    ;

builtin_type_reference
    : BUILTIN_TYPE generic_types?
    ;

declared_type_reference
    : declared_namespace '::' IDENTIFIER generic_types?
    ;
    
local_type_reference
    : THIS '::' IDENTIFIER generic_types?
    ;
    
generic_types
    : '<' type_reference (',' type_reference)* '>'
    ;

generic_attribute_reference
    : IDENTIFIER
    ;

declared_namespace
    : IDENTIFIER ('::' IDENTIFIER)*
    ;

// Type declaration

type_declaration 
    : DECLARE IDENTIFIER generic_type_attributes? '{' operation_statement+ '}'
    ;
        
generic_type_attributes
    : '<' IDENTIFIER (',' IDENTIFIER)* '>'
    ;

// Operation statements

if_statement
    : '(' expression ')' '=>' '{' operation_statement+ '}' ';'
    ;
 
 conditional_statement
    : if_statement
    ;
 
 data_statement
    : type_reference IDENTIFIER ';'
    ;
 
 operation_statement
    : conditional_statement
    | data_statement
    ;

// Expressions

// todo
expression
    : FALSE
    ;

// Member option parsing

member_options : OPTIONS OPTIONS_ENTER member_option* OPTIONS_EXIT ;
member_option : OPTION_KEY OPTION_SET option_value OPTION_END ;

// tood: support more than just string values
option_value
    : STRING_VAL
    ;

// Utility

namespace : IDENTIFIER ('.' IDENTIFIER)* 
          ;

directory : IDENTIFIER ('/' IDENTIFIER)* '/'
          ;
