parser grammar KryptonParser;

options 
    { tokenVocab=KryptonTokens; }

// Root members

init : import_statement* namespace_definition*
     ;

import_statement
    : IMPORT (directory)? IDENTIFIER '.' KPDL ';'
    ;

group_definition 
    : GROUP IDENTIFIER ';' 
    ;

protocol_definition 
    : PROTOCOL IDENTIFIER '{' message_definition? packet_definition* '}' 
    ;

message_definition 
    : IDENTIFIER (',' message_definition)?
    ;

packet_definition 
    : PACKET IDENTIFIER (':' packet_parent)? '{' operation_statement+ '}' 
    ;

packet_parent 
    : (namespace_reference '::')? IDENTIFIER (',' packet_parent)?
    ;

namespace_definition 
    : NAMESPACE IDENTIFIER '{' member_options? namespace_member* '}' 
    ;

namespace_member 
    : group_definition
    | namespace_definition
    | protocol_definition
    | type_declaration
    | packet_definition
    ;

// Types

type_name
    : IDENTIFIER
    | BUILTIN_TYPE
    ;

type_reference 
    : generic_attribute_reference
    | (namespace_reference '::')? type_name generic_types?
    ;
    
generic_attribute_reference
    : IDENTIFIER
    ;
    
generic_types
    : '<' type_reference (',' type_reference)* '>'
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

namespace_reference 
    : IDENTIFIER ('::' IDENTIFIER)* 
    | THIS ('::' IDENTIFIER)*
    ;

directory 
    : IDENTIFIER ('/' IDENTIFIER)* '/'
    ;
