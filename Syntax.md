# File Handling
    file_var = open file "file.txt"
    open file "file.txt" as file_var

# Function Definition
    function myFunction(param1, param2)
        // Do stuff
        return param1 ^ (param2 / 2)
    end

or
    
    function myFunction < param1, param2
        return 

# Object Definition
    object MyObject
        var = 3
        unassigned = nothing

        function aFunc1
            say "i am function 1"
        end
    end

# Package Importing
    use package Calculus
    use package Time

# Package Creation
    create package NewPackage

    function packageFunction
        say "aaa"
    end

    // Entire file contents become the package

# Creators
    plainList = list
    setList = unique list
    dictList = lookup list

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
    
    
    

   


    

