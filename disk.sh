function sysdisk() {
    local path;
    path="$(dirname -- "$PWD")";
    local executable="$path/rlsyscli/disk.sh";
    local result="$?";
    cd "$path/rlsyscli/rlsyscli" || return 1;
    
    for file in *.csproj;
        do if [ -f "$file" ];
            then dotnet run;
            cd ..;
            
            if [ "$result" -eq 0 ]
                then return 0;
                
            else
                echo "There was an error with reading the .csproj file!";
                return 1;
            fi      
        else
            echo "The directory doesn't contain a valid .csproj file!";
        fi;
    done;

    if [ -f "$executable" ];
        then chmod +x "$executable";
        ./"$executable";
        
        if [ "$result" -eq 0 ]
            then return 0;
            
        else
            echo "There was an error with reading the disk.sh file!";
            return 1;
        fi         
    else
        echo "The directory doesn't contain a valid disk.sh file!";
    fi;
}
