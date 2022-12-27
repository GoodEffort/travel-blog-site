$tripName = $args[0]
#echo "Trip name: $tripName"
$listFilePath = "./travel-blog-site/src/stores/$($tripName.toLower())-photos-list.ts"
#echo "List file path: $listFilePath"
$picPath = "./travel-blog-site/public/trips/$tripName/"
#echo "Pic path: $picPath"

$files = (Get-ChildItem -Path $picPath -File -Recurse).Name

Out-File -FilePath $listFilePath -InputObject "export const photosList = [" -Encoding utf8

$files | ForEach-Object {
    Out-File -FilePath $listFilePath -InputObject "    '$_'," -Encoding utf8 -Append
}

Out-File -FilePath $listFilePath -InputObject "];" -Encoding utf8 -Append
