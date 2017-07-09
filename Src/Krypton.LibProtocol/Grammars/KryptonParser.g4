parser grammar KryptonParser;

import TypeDeclaration;

options 
    { tokenVocab=KryptonTokens; }

// kpdl structure
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

// Utility

namespace : IDENTIFIER ('.' IDENTIFIER)* 
          ;

directory : IDENTIFIER ('/' IDENTIFIER)* '/'
          ;
