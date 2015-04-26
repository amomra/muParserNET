param($installPath, $toolsPath, $package, $project)
$dirX86 = $project.ProjectItems.Item("x86")
$dirX64 = $project.ProjectItems.Item("x64")

$fileX86 = $dirX86.ProjectItems.Item("muParser.dll")
$fileX64 = $dirX64.ProjectItems.Item("muParser.dll")

# set 'Copy To Output Directory' to 'Copy if newer'
$copyToOutputX86 = $fileX86.Properties.Item("CopyToOutputDirectory")
$copyToOutputX86.Value = 2
 
$copyToOutputX64 = $fileX64.Properties.Item("CopyToOutputDirectory")
$copyToOutputX64.Value = 2