# task-management Specification

## Purpose

Defines the behavior for managing tasks, including their attributes, persistence, and presentation in the UI.

## Requirements

### Requirement: Task has an optional description

A task SHALL support an optional short, free-text description in addition to its
title. The description MUST be limited to 500 characters. When no description is
provided, the task's description SHALL default to an empty value.

#### Scenario: Create a task with a description

- **WHEN** a task is created with a non-empty description
- **THEN** the task is stored with that description
- **AND** the created task returned by the API includes the description

#### Scenario: Create a task without a description

- **WHEN** a task is created with no description field (or an empty one)
- **THEN** the task is stored successfully with an empty description
- **AND** the API returns the task with an empty description

#### Scenario: Description exceeds the maximum length

- **WHEN** a task is created or updated with a description longer than 500 characters
- **THEN** the request is rejected with a validation error
- **AND** the task is not stored or modified

### Requirement: Task description is persisted on update

When an existing task is updated, the system SHALL persist the supplied
description, replacing any previously stored description.

#### Scenario: Update a task's description

- **WHEN** an existing task is updated with a new description
- **THEN** the stored task reflects the new description
- **AND** a subsequent read of the task returns the updated description

#### Scenario: Clear a task's description on update

- **WHEN** an existing task that has a description is updated with an empty description
- **THEN** the stored task's description becomes empty

### Requirement: Description is shown in the UI

The task UI SHALL allow a user to enter an optional description when adding a
task and SHALL display the description for any task that has one.

#### Scenario: Enter a description when adding a task

- **WHEN** the user types a description in the add-task form and submits
- **THEN** the new task is created with that description
- **AND** the description is displayed with the task in the list

#### Scenario: Task without a description

- **WHEN** a task has no description
- **THEN** no description text is shown for that task in the list

#### Scenario: Description input shows remaining characters and enforces the limit

- **WHEN** the user types in the description input
- **THEN** the number of remaining characters (out of 500) is shown near the input
- **AND** the input does not accept more than 500 characters
