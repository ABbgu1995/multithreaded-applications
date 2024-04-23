# SpreadsheetApp Project

## Overview
SpreadsheetApp is a C# application designed to manage a shared in-memory spreadsheet. It is optimized for concurrent use, allowing multiple threads to perform operations without conflicts, ensuring thread safety and avoiding deadlocks through meticulous synchronization strategies.

## Features
- **Thread-Safe Operations:** Utilizes mutexes/semaphores to synchronize access, ensuring that no two threads can perform conflicting operations at the same time.
- **Error Handling:** Throws exceptions for errors such as invalid parameters or out-of-bounds access.
- **Supports Multiple Concurrent Users:** Designed to be used by multiple users concurrently, mimicking functionalities similar to Google Docs and Sheets.

## Synchronization Strategy
- Detailed synchronization strategies include the use of mutexes to protect individual cells or groups of cells, rather than locking the entire spreadsheet, to enhance performance and responsiveness.
- Mutexes are strategically placed to guard critical sections where spreadsheet cells are accessed or modified.

## Simulator
- A console application named `Simulator` tests the spreadsheet object under stress conditions to ensure robustness in multi-threaded environments.
- Supports multiple users performing a variety of operations on the spreadsheet concurrently.

## GUI Application
- Provides a basic graphical user interface using controls like DataGridView for spreadsheet interaction.
- Supports basic operations such as load, save, and update cells visually.

## Documentation
- The `report.pdf` includes a detailed description of the internal object design, synchronization mechanisms, and a diagram illustrating the lock types and placements.
- Screenshots and screen recordings within the documentation demonstrate the application features and usage.

## Project Structure
- **SharableSpreadSheet.cs:** Core class managing the spreadsheet logic.
- **Simulator:** Tests the SharableSpreadSheet class with multiple threads to simulate a multi-user environment.
- **SpreadsheetApp:** Windows form application providing a GUI for the spreadsheet.

## Setup and Running
- Clone the repository and open the solution in Visual Studio.
- Build and run the `SpreadsheetApp` for GUI interaction or `Simulator` for command-line testing.

Good luck and enjoy managing your spreadsheets efficiently and effectively!
