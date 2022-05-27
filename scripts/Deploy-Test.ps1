param (
    [string] $RepoName
)

if (!$RepoName) {
    Write-Host "Must provide a repository name"
    Exit 1
}

$orgName = "g-wuk"
$orgRepoName = "$orgName/$RepoName"
$repoUrl = "https://github.com/$orgRepoName.git"

Write-Host "Creating repository $repoUrl"

git checkout master
git branch -D test

git checkout --orphan test
git add -A
git commit -am "Starting point"

gh auth login
gh repo create $orgRepoName --private

git remote add $RepoName $repoUrl
git push -u $RepoName test

git checkout master
git remote remove $RepoName
git branch -D test

