﻿namespace TravelConnect.Domain.Exceptions;

public class ValidationException(string message) : DomainException(message)
{
}
