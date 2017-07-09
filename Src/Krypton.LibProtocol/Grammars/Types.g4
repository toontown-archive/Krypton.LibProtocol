parser grammar Types;

options 
    { tokenVocab=KryptonTokens; }

// Refrences

type_reference 
    : PRIMITIVE
    | declared_type_reference
    ;
    
declared_type_reference
    : IDENTIFIER
    | declared_generic_type_reference
    ;
    
declared_generic_type_reference
    : IDENTIFIER '<' type_reference '>'
    ;