$StackName = "vanlune-products"

rm -r  C:\Projects\vanlune\vanlune-products\Products.Application\bin\Release\netcoreapp3.1\publish\

dotnet publish C:\Projects\vanlune\vanlune-products -c release

$7zipPath = "$env:ProgramFiles\7-Zip\7z.exe"
if (-not (Test-Path -Path $7zipPath -PathType Leaf)) {
    throw "7 zip file '$7zipPath' not found"
}
Set-Alias 7zip $7zipPath
$Source = "C:\Projects\vanlune\vanlune-products\Products.Application\bin\Release\netcoreapp3.1\publish\*"
$Target = "C:\Projects\vanlune\vanlune-products\Products.Application\bin\Release\netcoreapp3.1\publish\Products.zip"

7zip a -mx=9 $Target $Source

Get-Job | Wait-Job

aws s3 cp C:/Projects/vanlune/vanlune-products/Products.Application/template-products.yaml s3://vanlune-bin-dev
aws s3 cp C:\Projects\vanlune\vanlune-products\Products.Application\bin\Release\netcoreapp3.1\publish\Products.zip s3://vanlune-bin-dev  
Get-Job | Wait-Job


$exists = aws cloudformation describe-stacks --stack-name $StackName
if ($exists)
{
	aws cloudformation  update-stack --stack-name $StackName --template-url https://vanlune-bin-dev.s3.amazonaws.com/template-products.yaml --capabilities CAPABILITY_IAM CAPABILITY_AUTO_EXPAND
}
else
{
	aws cloudformation create-stack  --stack-name $StackName --template-url https://vanlune-bin-dev.s3.amazonaws.com/template-products.yaml --capabilities CAPABILITY_IAM CAPABILITY_AUTO_EXPAND
}
aws cloudformation wait stack-update-complete --stack-name $StackName

aws lambda update-function-code --function-name vanlune-category-create                 --s3-bucket vanlune-bin-dev --s3-key Products.zip
aws lambda update-function-code --function-name vanlune-category-delte                  --s3-bucket vanlune-bin-dev --s3-key Products.zip
aws lambda update-function-code --function-name vanlune-category-get                    --s3-bucket vanlune-bin-dev --s3-key Products.zip
aws lambda update-function-code --function-name vanlune-category-get-all                --s3-bucket vanlune-bin-dev --s3-key Products.zip
aws lambda update-function-code --function-name vanlune-category-get-by-games           --s3-bucket vanlune-bin-dev --s3-key Products.zip
aws lambda update-function-code --function-name vanlune-category-update                 --s3-bucket vanlune-bin-dev --s3-key Products.zip
aws lambda update-function-code --function-name vanlune-customize-create                --s3-bucket vanlune-bin-dev --s3-key Products.zip
aws lambda update-function-code --function-name vanlune-customize-delete                --s3-bucket vanlune-bin-dev --s3-key Products.zip
aws lambda update-function-code --function-name vanlune-customize-get-by-filters        --s3-bucket vanlune-bin-dev --s3-key Products.zip
aws lambda update-function-code --function-name vanlune-customize-update                --s3-bucket vanlune-bin-dev --s3-key Products.zip
aws lambda update-function-code --function-name vanlune-games-get-all                   --s3-bucket vanlune-bin-dev --s3-key Products.zip
aws lambda update-function-code --function-name vanlune-product-create                  --s3-bucket vanlune-bin-dev --s3-key Products.zip
aws lambda update-function-code --function-name vanlune-product-delete-all              --s3-bucket vanlune-bin-dev --s3-key Products.zip
aws lambda update-function-code --function-name vanlune-product-get-all-by-category     --s3-bucket vanlune-bin-dev --s3-key Products.zip
aws lambda update-function-code --function-name vanlune-product-get-all-by-tag          --s3-bucket vanlune-bin-dev --s3-key Products.zip
aws lambda update-function-code --function-name vanlune-product-get-by-filters          --s3-bucket vanlune-bin-dev --s3-key Products.zip
aws lambda update-function-code --function-name vanlune-product-get-by-id               --s3-bucket vanlune-bin-dev --s3-key Products.zip
aws lambda update-function-code --function-name vanlune-product-update                  --s3-bucket vanlune-bin-dev --s3-key Products.zip
aws lambda update-function-code --function-name vanlune-product-upload-image            --s3-bucket vanlune-bin-dev --s3-key Products.zip
aws lambda update-function-code --function-name vanlune-tags-create                     --s3-bucket vanlune-bin-dev --s3-key Products.zip
aws lambda update-function-code --function-name vanlune-tags-get-all                    --s3-bucket vanlune-bin-dev --s3-key Products.zip
aws lambda update-function-code --function-name vanlune-variants-create                 --s3-bucket vanlune-bin-dev --s3-key Products.zip
aws lambda update-function-code --function-name vanlune-variants-get-all                --s3-bucket vanlune-bin-dev --s3-key Products.zip
aws lambda update-function-code --function-name vanlune-variants-servers-get-all        --s3-bucket vanlune-bin-dev --s3-key Products.zip