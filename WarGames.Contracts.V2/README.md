# Experience
One of my personal pet peeves, we are willing to isolate layers and create new projects "as needed". When it comes to model changes we demand to change the existing models.
"So now in one swoop we must switch EVERYTHING?" "Balderdash!"

I have seen more bugs come out of model and contract changes than any other type of modification. It has always classically delayed an entire sprint or feature of value.

# Steps
_I hope you have unit tests :D_
1. Create a new contract project.
1. Create the new contracts.
1. Mark deprecating contracts. _the IDE is very good at warning you where the deprecations are_
1. If you're not confident you can complete the swaps cleanly, create maps between the old and new contracts.
1. Swap out old model usage. _This can be done slowly AND new content can use the new contracts_.
1. _If you have unit tests run them now_

# Benefits

IMO you get the following benefits.

* Explicit contract deprecation.
  * New work can chose the new contract.
* The ability to pace your changes.
* Reduced or no conflicts.
* You may realize you need both contracts.

# Drawbacks

* One extra project and namespace.
* Extra maps if the work at hand is significant.
  * _Maps are cheap to implement._


# Anecdotes
## Explicit contract deprecation
On every project where we had multiple developers in the same area, even though we KNEW there was a model or contract change coming
the massive change always wrecked our assumptions as we were coding since we could only ever have 1 master. 

This forced us to:
* Wait for the model updates to be complete because we need the model change.
* Have a massive conflict on every thing we touched and then wait for the model updates to be complete.
* Wait for the model updates because we keep running into conflicts during the refactor.

### New work can chose the new contract
I was once in a project with around 490 different ASPX Forms, many (not all) of which required immediate changes to the common models shared between them. The rest of the pages could eventually use those changes.

Changing the model directly would require all of the pages eat the change right away whether they needed it or not.

I was also once in a multi-microservice system that was fast and loose with the contracts. Without the new contract the team kept using the old in other microservices, immediately doubling the work needed to be done
since every usage of the wrong contract had to be replaced in one go, with the new contract.

## Reduced or no conflicts
Because each area is left to chose the new versus old contract, the person doing the deprecation is only responsible for the old areas impacted by the change. 

The active developers modifying existing areas or creating new ones can switch to the new contracts when they get to them.

## You may realize you need both contracts
"But I need this!" "But I need that!" "People! People! We have enough models for everyone."

Single Responsibilty applies to contracts also. When we were combining new details because one of our dozens of downstream systems needed one more thing we ended up with a 2MB monstrosity that tied into 
every layer across domains it should never cross into. The effort to split the model out into smaller pieces was no small task.

We are changing the contract for a reason and that reason may be because there is more than one stakeholder asking for changes and the Product Owner/Business Analyst has yet to identify that fact.