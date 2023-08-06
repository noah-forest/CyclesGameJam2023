using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class StinkerSpray : MonoBehaviour
{
    public GameObject stinker;
    private int killedStinkers = 0;
    public int stinkerCount = 2;
    public int maxStinkers = 5;
    
    private bool _dragging = false;
    private bool previouslyLetGo = false;
    private Vector3 _offset;
    private Rigidbody2D rb;
    private Camera _camera;
    public ParticleSystem sprayParticles;
    public Transform raycastSprayFrom;
    private AudioSource spraySound;

    public GameObject grabText;
    public GameObject sprayText;
    public bool showSprayText = true;

    public bool facingLeft = false;
    public Transform sprayBody;
    
    private Vector3 initialMousePosition;
    private Vector3 lastMousePosition;
    private float throwForceMultiplier = 30f; // You can adjust this value to control the throw force.
    public float gravityMultiplier = 1;
    private bool _respawning = false;
    public float edgeThickness = 1f; // Adjust this value to control the thickness of the edges

    void Start()
    {
        stinkerCount += GameManager.singleton.difficulty;
        stinkerCount = Mathf.Clamp(stinkerCount, 0, maxStinkers);   
        for (int i = 0; i < stinkerCount; i++)
        {
            GameObject stink = Instantiate(stinker);
            stink.SetActive(true);
            Stinker stinkComponent = stink.GetComponent<Stinker>();
            stinkComponent.OnKilled.AddListener(() =>
            {
                killedStinkers += 1;
                if (killedStinkers >= stinkerCount)
                {
                    GameManager.singleton.FinishMiniGame();
                }
                stinkComponent.OnKilled.RemoveAllListeners();
            });
        }
        spraySound = GetComponent<AudioSource>();
        _camera = Camera.main;
        rb = GetComponent<Rigidbody2D>();


        float halfScreenWidth = _camera.orthographicSize * _camera.aspect;
        float halfScreenHeight = _camera.orthographicSize;
        CreateEdgeObject(new Vector3(0f, halfScreenHeight + edgeThickness / 2f, 0f), new Vector2(halfScreenWidth * 2f, edgeThickness), "TopEdge");
        CreateEdgeObject(new Vector3(0f, -halfScreenHeight - edgeThickness / 2f, 0f), new Vector2(halfScreenWidth * 2f, edgeThickness), "BottomEdge");
        CreateEdgeObject(new Vector3(-halfScreenWidth - edgeThickness / 2f, 0f, 0f), new Vector2(edgeThickness, halfScreenHeight * 2f), "LeftEdge");
        CreateEdgeObject(new Vector3(halfScreenWidth + edgeThickness / 2f, 0f, 0f), new Vector2(edgeThickness, halfScreenHeight * 2f), "RightEdge");
    }

    void Update()
    {
        // Check if left mouse button is being held
        if (Input.GetMouseButtonDown(0))
        {
            if (showSprayText)
            {
                showSprayText = false;
                grabText.SetActive(false);
                sprayText.SetActive(true);
            }
            // Cast a ray from the mouse position to see if it hits the GameObject
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

            if (hit.collider && hit.collider.gameObject == gameObject)
            {
                rb.velocity = Vector3.zero;
                rb.gravityScale = 0;
                // If the mouse hits the GameObject, start grabbing it
                _dragging = true;
                _offset = transform.position - _camera.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        
        // While holding the left mouse button, update the Rigidbody2D's position based on the mouse cursor's position
        if (_dragging)
        {
            if (Input.GetMouseButtonUp(0))
            {
                // If the left mouse button is released, stop grabbing the GameObject
                _dragging = false;
                previouslyLetGo = true;

                // Calculate the velocity based on the mouse speed and apply it to the Rigidbody2D.
                Vector3 throwVelocity = (lastMousePosition - _camera.ScreenToWorldPoint(Input.mousePosition)) * throwForceMultiplier;
                rb.velocity = -throwVelocity;
                rb.gravityScale = 1;
            }
            lastMousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButtonDown(1))
            {
                Spray();
            }
            Vector3 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
            //rb.MovePosition(mousePos + _offset);
            rb.velocity = (mousePos + _offset - transform.position) * 20f; // Adjust the multiplier to control the throw speed.
            
            SetDirection(rb.velocity.x > 0);

            // Scale the object based on its vertical position relative to the screen
            /*float viewportY = _camera.WorldToViewportPoint(transform.position).y;
            float scale = Mathf.Lerp(2f, 0.2f, viewportY);
            transform.localScale = new Vector3(scale, scale, 1.0f);*/
        }

        if (IsOffscreen())
        {
        }
    }

    private void BounceOffScreenEdges()
    {
        Vector3 viewportPos = -_camera.WorldToViewportPoint(transform.position);

        if (viewportPos.x < 0f || viewportPos.x > 1f)
        {
            // Object hits left or right edge of the screen, reverse the X velocity to bounce it off
            rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
        }

        if (viewportPos.y < 0f || viewportPos.y > 1f)
        {
            // Object hits top or bottom edge of the screen, reverse the Y velocity to bounce it off
            rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);
        }
    }
    public GameObject edgePrefab; // Drag an empty GameObject prefab here in the Inspector

    
    private void CreateEdgeObject(Vector3 position, Vector2 size, string name)
    {
        GameObject edgeObject = Instantiate(edgePrefab, position, Quaternion.identity);
        edgeObject.name = name;
        BoxCollider2D boxCollider = edgeObject.GetComponent<BoxCollider2D>();
        boxCollider.size = size;
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2);
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
        rb.MovePosition(Vector2.zero);
        _respawning = false;
    }

    private void FixedUpdate()
    {
        if (previouslyLetGo)
        {
            //rb.velocity += (Vector2.up * Physics2D.gravity*gravityMultiplier);
        }
    }

    private void SetDirection(bool left)
    {
        facingLeft = left;
        if (left)
        {
            sprayParticles.transform.rotation = Quaternion.Euler(0, 180, 0);
            sprayBody.localScale = new Vector3(-Mathf.Abs(sprayBody.localScale.x), sprayBody.localScale.y, sprayBody.localScale.z);
        }
        else
        {
            sprayParticles.transform.rotation = Quaternion.Euler(0, 0, 0);
            sprayBody.localScale = new Vector3(Mathf.Abs(sprayBody.localScale.x), sprayBody.localScale.y, sprayBody.localScale.z);
        }
    }

    private void Spray()
    {
        sprayText.SetActive(false);

        //AudioSource.PlayClipAtPoint(spraySound.clip, transform.position, 0.6f);
        spraySound.pitch = Random.Range(1.1f, 1.4f);
        spraySound.PlayOneShot(spraySound.clip);
        ParticleSystem p = Instantiate(sprayParticles);
        p.transform.position = sprayParticles.transform.position;
        p.Play();
        SprayRaycast();
    }

    private void SprayRaycast()
    {
        Vector3 raycastDirection = -raycastSprayFrom.right;
        if (facingLeft)
        {
            raycastDirection = -raycastDirection;
        }
        float raycastDistance = 1.7f;
        RaycastHit2D straightHit = Physics2D.Raycast(raycastSprayFrom.position, raycastDirection, raycastDistance);

        // Raycast 30 degrees upward
        float angleUp = 17f;
        Vector2 directionUp = Quaternion.Euler(0f, 0f, angleUp) * raycastDirection;
        RaycastHit2D upHit = Physics2D.Raycast(raycastSprayFrom.position, directionUp, raycastDistance);

        // Raycast 30 degrees downward
        float angleDown = -17f;
        Vector2 directionDown = Quaternion.Euler(0f, 0f, angleDown) * raycastDirection;
        RaycastHit2D downHit = Physics2D.Raycast(raycastSprayFrom.position, directionDown, raycastDistance);

        // Draw the rays and perform actions based on the hit information
        DrawRay(raycastSprayFrom.position, -raycastSprayFrom.right * raycastDistance, Color.green);
        DrawRay(raycastSprayFrom.position, directionUp * raycastDistance, Color.green);
        DrawRay(raycastSprayFrom.position, directionDown * raycastDistance, Color.green);
        Stinker comp = null;
        if (straightHit.collider)
        {
            straightHit.collider.TryGetComponent<Stinker>(out comp);
        }

        if (upHit.collider)
        {
            upHit.collider.TryGetComponent<Stinker>(out comp);
        }

        if (downHit.collider)
        {
            downHit.collider.TryGetComponent<Stinker>(out comp);
        }

        if (comp)
        {
            comp.Spray();
        }
    }

void DrawRay(Vector3 start, Vector3 dir, Color color)
{
    //Debug.DrawRay(start, dir, color);
}
    
    private bool IsOffscreen()
    {
        Vector3 viewportPos = _camera.WorldToViewportPoint(transform.position);
        return (viewportPos.x < 0f || viewportPos.x > 1f || viewportPos.y < 0f || viewportPos.y > 1f);
    }
}
