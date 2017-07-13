parser grammar KryptonParser;

options 
    { tokenVocab=KryptonTokens; }

// Root members

init : imports+=import_statement* groups+=group_definition* libraries+=library_definition* protocols+=protocol_definition*
     ;

import_statement : IMPORT (path=directory)? file=IDENTIFIER '.' KPDL ';'
                 ;

group_definition : GROUP name=IDENTIFIER ';' ;

library_definition : LIBRARY name=IDENTIFIER '{' library_statement* '}' ;

library_statement
    : packet_definition
    | type_declaration
    ;

protocol_definition : PROTOCOL 
                      ns=namespace '[' name=IDENTIFIER ']'
                      '{' message_definitions? packet_definition* '}' ;

message_definitions : name=IDENTIFIER (',' message_definitions)?
                    ;

packet_definition : PACKET name=IDENTIFIER (':' packet_parent)? 
                    '{' operation_statement+ '}' ;

packet_parent : ns=namespace '[' name=IDENTIFIER ']' (',' packet_parent)?
              ;

// Types

type_reference 
    : primitive_type_reference
    | generic_primitive_type_reference
    | declared_type_reference
    | declared_generic_type_reference
    | local_type_reference
    | local_generic_type_reference
    | generic_attribute_reference
    ;

primitive_type_reference
    : PRIMITIVE
    ;
    
generic_primitive_type_reference
    : GENERIC_PRIMITIVE '<' type_reference (',' type_reference)* '>'
    ;

declared_type_reference
    : namespace '::' IDENTIFIER
    ;
    
declared_generic_type_reference
    : namespace '::' IDENTIFIER '<' type_reference (',' type_reference)* '>'
    ;
    
local_type_reference
    : THIS '::' IDENTIFIER
    ;
    
local_generic_type_reference
    : THIS '::' IDENTIFIER '<' type_reference (',' type_reference)* '>'
    ;

generic_attribute_reference
    : IDENTIFIER
    ;

// Type declaration

type_declaration 
    : DECLARE IDENTIFIER generic_type_attributes? '{' operation_statement+ '}'
    ;
 
        
generic_type_attributes
    : '<' generic_type_attribute (',' generic_type_attribute)* '>'
    ;

generic_type_attribute
    : IDENTIFIER
    ;

// Operation statements

conditional_value
    : TRUE 
    | FALSE 
    | INTEGER 
    | IDENTIFIER 
    ;

if_statement
    : conditional_value OPERATOR conditional_value
    ;
 
 conditional
    : if_statement
    ;
 
 conditional_statement
    : '(' conditional ')' '=>' '{' operation_statement+ '}' ';'
    ;
 
 meta_operation
    : IDENTIFIER METAOPERATOR ';'
    ;
 
 data_statement
    : type_reference IDENTIFIER ';'
    ;
 
 operation_statement
    : conditional_statement
    | meta_operation
    | data_statement
    ;

// Utility

namespace : IDENTIFIER ('.' IDENTIFIER)* 
          ;

directory : IDENTIFIER ('/' IDENTIFIER)* '/'
          ;
