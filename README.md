# Dependencies
- I'm using .NET SDK: Version: 9.0.102 (Apologies in advance if y'all need to upgrade to this vers)

# Collaboration Rules

## 1. Team Branch Rules
- Each team has their own branch (e.g., `module1-team4`), and **only members of the respective team** can edit their branch.
- Changes made within your team branch **do not require a pull request**. Teams are responsible for maintaining the quality and integrity of their branch.
- **Feature branches**: Team members may create feature branches (e.g., `feature-login`) within their own team branch if needed for better collaboration. These can be merged back into the team branch without a pull request.

## 2. Module Branch Rules
- Each module has a dedicated module branch (e.g., `module1`), which acts as a staging area for combining the work of the two teams assigned to the module.
- **Merging from a team branch to the module branch requires a pull request**. 
- At least one member from the other team working on the same module must review and approve the pull request.

## 3. Main Branch Rules
- The `main` branch is reserved for **the bootstrap template and the base MVC project**.
- **Merging into `main` requires**:
  1. A pull request.
  2. Approval from **all group leaders** working on the module.
But it is highly unlikely we will ever be merging into main.

## 4. General Guidelines
- **Responsibility**:
  - You and your team is responsible for your branch/module changes. Everything is logged in GitHub. If you have any disputes, bring it up with your team leader.
- **Commit Messages**:
  - Write clear and descriptive commit messages (e.g., `Fix bug in login validation` or `Add API endpoint for user data retrieval`).
- **Code Reviews**:
  - Be constructive and respectful when reviewing others' code.
  - Provide specific feedback and suggest improvements where necessary.
- **Conflict Resolution**:
  - If a conflict arises during a merge, work with your team or the other team to resolve it. If needed, escalate to the instructor or project lead.
- **Testing**:
  - Always test your code locally before pushing changes.

## 5. Example Workflow

### Within Your Team Branch (e.g., `module1-team4`)
1. Pull the latest changes:
   ```
   bash
   git pull origin module1-team4
   ```

2. Create a new branch to work on your feature:
   ```
   git checkout -b feature-login
   ```
3. Make changes, test them locally, and commit:
   ```
   git add .
   git commit -m "Add login feature"
   git push origin feature-login
   ```
4. Merge your feature branch back into your team branch:
   ```
   git checkout module1-team4
   git merge feature-login
   git push origin module1-team4
   ```
# Files
- startbootstrap-sb-admin-2-gh-pages (Bootstrap template: refer to this for your example pages. Use/link them according to your team's needs.)
- CleanBrilliantCompany (Generated MVC project template: This is THE project itself)
