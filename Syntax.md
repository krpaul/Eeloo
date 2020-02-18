# File Handling
    file_var = open file "file.txt"
    open file "file.txt" as file_var

# Function Definition
    function myFunction(param1, param2)
        // Do stuff
        return param1 ^ (param2 / 2)
    end

or (alternate syntax)
    
    function myFunction < param1, param2
        return 
    end
    
    function addFunc(a, b)
        return a + b
    end
    say(1, 5) // -> 6
    
    function returnVals < a, b
        return a, b
    end
    say(returnVals(5, "a")) // -> [5, "a"]

# Object Definition (not implemented)
    object MyObject
        var = 3
        unassigned = nothing

        function aFunc1
            say "i am function 1"
        end
    end

# Package Importing (not implemented)
    use package Calculus
    use package Time

# Package Creation (not implemented)
    create package NewPackage

    function packageFunction
        say "aaa"
    end

    // Entire file contents become the package

# Creators
    plainList = new list
    set = unique list
    s = string
    
    // All throw errors if assigned a 
    n_num = negative number
    p_num = positive number
    e_num = even number
    o_num = odd number
     
    
# Comments
## Single Line
    // Comment
    
## Multi Line
    start comment
    This is a 
    multiline
    comment
    end comment
    
# Lists
    a_list = unique list
    a_list = [3, 3] // Throws error; violates 'unique' constraint
    a_list = [3, 5] // Works    
    
    a_list[1] = 13
    say(a_list) // -> '[3, 13]'
    
# Loops
    x = 0
    until x isn't 4
        x += 1
    end
-----    
    x = 0
    whilst x is less than 4
        x += 1
    end
-----
    from 0 to 4 use x
        say(x)
    end
-----
    repeat 4 times // -> has no iterator variable
        say("loop")
    end
-----
    for each x in 0...4
        say(x)
    end
-----
    for each x in 0 to 100 by 5 // -> multiples of 5 to 100
        say(x)
    end 
-----
    
# If Statments
    x = 4
    if x isn't 5
        say("x isn't 5")
    else if x is 6
        say("x is 6")
    else
        say("x is something else")
    end
    
# Ranges
    r1 = 1...5 // -> [1, 2, 3, 4, 5]
    r2 = 1...5 by 2 // -> [1, 3, 5]
    r3 = 1 to 10 by 3 // -> [1, 4, 7, 10]

# Aliasing
These are alternate phrases for lexer keywords and defined functions

Groups of equivalent phrases include:

    say("something")
    print("something") 
    output("somthing")
    
    function aFunc(a, b) ...
    procedure bFunc(a, b) ...
    
    4 / 5
    4 divided by 5
    
    4 * 5
    4 multiplied by 5 
    
    for each x in 1...5
    iterate x 1...5
    for x in 1...5
    
    stop()
    exit()
    
    return 1
    give 1
    yield 1
    
etc.
    

