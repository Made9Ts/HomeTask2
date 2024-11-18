using System;
using System.Collections.Generic;

// Model
public class Contact
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
}

public interface IContactRepository
{
    void AddContact(Contact contact);
    List<Contact> GetContacts();
    void UpdateContact(Contact contact);
    void DeleteContact(int id);
}

// Реализация хранилища контактов в памяти
public class ContactRepository : IContactRepository
{
    private readonly List<Contact> _contacts = new List<Contact>();
    private int _nextId = 1;

    public void AddContact(Contact contact)
    {
        contact.Id = _nextId++;
        _contacts.Add(contact);
    }

    public List<Contact> GetContacts()
    {
        return _contacts;
    }

    public void UpdateContact(Contact contact)
    {
        var existingContact = _contacts.Find(c => c.Id == contact.Id);
        if (existingContact != null)
        {
            existingContact.Name = contact.Name;
            existingContact.Phone = contact.Phone;
            existingContact.Email = contact.Email;
        }
    }

    public void DeleteContact(int id)
    {
        var contact = _contacts.Find(c => c.Id == id);
        if (contact != null)
        {
            _contacts.Remove(contact);
        }
    }
}

public class ContactService
{
    private readonly IContactRepository _contactRepository;

    public ContactService(IContactRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }

    public void AddContact(Contact contact)
    {
        _contactRepository.AddContact(contact);
    }

    public List<Contact> GetAllContacts()
    {
        return _contactRepository.GetContacts();
    }

    public void UpdateContact(Contact contact)
    {
        _contactRepository.UpdateContact(contact);
    }

    public void DeleteContact(int id)
    {
        _contactRepository.DeleteContact(id);
    }
}

// View
public class ContactView
{
    public void DisplayContacts(List<Contact> contacts)
    {
        Console.WriteLine("Список контактов:");
        foreach (var contact in contacts)
        {
            Console.WriteLine($"{contact.Id}: {contact.Name}, {contact.Phone}, {contact.Email}");
        }
    }
}

// Controller
public class ContactController
{
    private readonly ContactService _contactService;
    private readonly ContactView _contactView;

    public ContactController(ContactService contactService, ContactView contactView)
    {
        _contactService = contactService;
        _contactView = contactView;
    }

    public void ShowContacts()
    {
        var contacts = _contactService.GetAllContacts();
        _contactView.DisplayContacts(contacts);
    }

    public void AddContact(Contact contact)
    {
        _contactService.AddContact(contact);
    }

    public void DeleteContact(int id)
    {
        _contactService.DeleteContact(id);
    }
}

// Точка входа
class Program
{
    static void Main(string[] args)
    {
        // Создаем репозиторий, сервис, представление и контроллер
        IContactRepository contactRepository = new ContactRepository();
        ContactService contactService = new ContactService(contactRepository);
        ContactView contactView = new ContactView();
        ContactController contactController = new ContactController(contactService, contactView);

        // Добавляем несколько контактов
        contactController.AddContact(new Contact { Name = "Иван Иванов", Phone = "123-456-7890", Email = "ivan@example.com" });
        contactController.AddContact(new Contact { Name = "Мария Петрова", Phone = "098-765-4321", Email = "maria@example.com" });

        // Отображаем список контактов
        contactController.ShowContacts();

        // Удаляем контакт
        contactController.DeleteContact(1); // Удаляем контакт с Id = 1

        // Отображаем обновленный список контактов
        Console.WriteLine("\nПосле удаления:");
        contactController.ShowContacts();
    }
}