function sysdisk() {
    local path;
    local result="$?";
    
    if [ "$OSTYPE" == "linux-gnu" ]
        then path="$(dirname -- "$PWD")";
    fi;
    
    if [ "$OSTYPE" == "msys" ]
        then path="$(pwd -W)/";
    fi;
    
    local executable="$path/rlsyscli/disk.sh";
    cd "$path/rlsyscli/rlsyscli" || return 1;
    
    for file in *.csproj;
        do if [ -f "$file" ];
            then dotnet run;
            cd ..;
            return $result;
            
        else
            echo "The directory doesn't contain a valid .csproj file!";
            return 1;
        fi;
    done;

    if [ -f "$executable" ];
        then chmod +x "$executable";
        ./"$executable";
        return $result;
      
    else
        echo "The directory doesn't contain a valid disk.sh file!";
        return 1;
    fi;
}
