## Cheats

Some cheats are included in the game to make testing easier. The cheats only work on Scene's containing a GameController with the cheatsEnabled bool checked.

Press a key below for the following effect

r - Restart the current level
` - Load the menu scene
1 - Load the first level (According to the order in Build Settings)
2 - Load the second level...
3 - Load the third level...
4 - Load the fourth level...
5 - Load the fifth level...
0 - Load the prototype level (Doesn't seem to be working on every scene)
j - Alternate "Moon" Jump that can be infinitely repeated to easily access any spot on the map
+ - Increases the player's maximum speed
- - Decreases the player's maximum speed
t - Teleport the player to the goal
m - Increase the number of PickUps the player has collected
y - Reset the timer to 0


## Git

For a quick introduction to git, try this short [http://try.github.io/levels/1/challenges/1](interactive tutorial).

We are using a simpler version of the gitflow branching structure, described [http://nvie.com/posts/a-successful-git-branching-model/](here).

We have two main branches, develop and master, and may have a number of temporary feature branches from time to time.

You can use `git branch` to see the list of branches you are on, and `git checkout BRANCH_NAME` to checkout a specific branch.

1) Develop: This is the default branch, and the one you should be on most of the time. Whenever you do work, check in to this branch, unless you are on a feature branch. By definition, develop should be the most unstable branch. 
1) Master: This branch won't be updated as often, and is hopefully more stable. I will merge develop into master whenever we have a release due.

### Daily git workflow

At the beginning of every work session you should run `git pull` inside the project directory to pull the most recent version of the project to your machine. 

After you have made some changes, it is time to commit them.

1) run `git status` to see an output of all the files that have changed.
1) run `git add MODIFIED FILE` for each file modified.
1) run `git status` again to make sure all of your changes are staged.
1) run `git commit -m "MESSAGE" do commit your changes with MESSAGE describing your changes.

At the end of the day, you can push your changes to the server with `git push` (you may have to `git pull` first).

### Feature Branches

Lets say you have some big feature you want to work on, it might take multiple days and you want to be able to check in changes without affecting the rest of develop (As the rest of the team will be making other changes). This is when you create a feature branch, with `git checkout -B BRANCH_NAME`. This will create BRANCH_NAME that is identical to the branch you are on, and it will check out the new branch for you. You can make as many changes as you want to this new branch and develop won't be affected.

When your feature is complete, make sure your branch is pushed to github (`git push origin BRANCH_NAME`), and then it is ready to be merged. Go to the github project [https://github.com/snelson3/happyyarnball](page), and create a pull request. Somebody else will then be able to review the changes made, and merge the branch into develop. Once this is done, the temporary feature branch will be deleted.

## Issues

Github has a very nice issue tracker, for internally tracking/reporting bugs. If you encounter a bug, or think of an feature that should be added, go to [https://github.com/snelson3/happyyarnball/issues](issues) on Github, and create an issue. Describe what the issue is (if it is a bug include precise steps to reproduce), and add tags (there are tags for  bug/enhancement as well as how the issue should be prioritized), and you can assign it to be someones responsibility to fix.


# Notes

Notes:
There are deviations from the architecture notes in the script names and quantity. "Apply[Effect]" has been consolidated into the DestroyByContact script, and the DestroyByContact scripts will be re-written for each powerup prefab to apply the desired effects. This will be easier to manage. Also of note is that "Rotator" is now "MorselAnimator". This decision was made under the realization than any number of objects may require rotations, and so "Rotator" is too general for script management.

The following are discussed interfaces used for cross-script functionality. These function names and types will remain permanent so as to maintain consistency between modules.

PlayerController:
SetSpeed(float) - Used by powerup to set player speed.
GetSpeed() - Useful getter method for implementation-independent speed altering.

Example uses:
player.GetComponent<PlayerController>().SetSpeed(50); //sets player speed to 50.
layer.GetComponent<PlayerController>().SetSpeed(2 * PlayerController.GetSpeed()); //doubles player's current speed.



GameController:
AddMorsel() - Used for incrementing the morsel count for the level.
PlayerDestroy() - Used for handling player death event. Used by level boundary box script and any player-killing level structures.
ExitReached() - Used for telling the game manager to end the level.

Example uses:
gc.GetComponent<GameController>().PlayerDestroy();
gc.GetComponent<GameController>().AddMorsel(); //upon entering a morsel's collider.
gc.GetComponent<GameController>().ExitReached(); //upon entering the game exit.



CameraController:
SetPlayer(GameObject) - Used for removing player GameObject pointer upon player death,
prevents camera from following dead player or attempting to follow non-existing player GameObject.

Example uses:
camera.GetComponent<CameraController>().SetPlayer(null); //upon player death, used for CameraController conditional check before updating position.
camera.GetComponent<CameraController>().SetPlayer((GameObject)Instantiate(player), startPosition, startRotation); //upon player respawn, if needed.