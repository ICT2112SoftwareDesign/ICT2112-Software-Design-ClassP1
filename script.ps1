$files = Get-ChildItem -Recurse -Include *.cs -File

foreach ($file in $files) {
    $content = Get-Content $file.FullName
    $currentClass = ""
    $methods = @()

    foreach ($line in $content) {
        # Match class name
        if ($line -match '^\s*(public|private|internal|protected)?\s*(abstract\s+|static\s+)?class\s+(\w+)') {
            if ($currentClass -ne "") {
                # Print previous class's methods
                Write-Output "`n======================="
                Write-Output "Class: $currentClass"
                Write-Output "======================="
                $methods | ForEach-Object { Write-Output $_ }
                $methods = @()
            }
            $currentClass = $Matches[3]
        }

        # Match method signatures
        if ($line -match '^\s*(public|private|protected|internal)?\s*(static\s*)?(async\s*)?\w+(\<.*?\>)?\s+\w+\s*\(.*?\)\s*{?') {
            $methods += $line.Trim()
        }
    }

    # Print remaining methods for the last class
    if ($currentClass -ne "" -and $methods.Count -gt 0) {
        Write-Output "`n======================="
        Write-Output "Class: $currentClass"
        Write-Output "======================="
        $methods | ForEach-Object { Write-Output $_ }
    }
}