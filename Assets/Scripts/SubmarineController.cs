using System;
using UnityEngine;

/// <summary>
/// Défi 4 - Sébastien Bruere
/// Réécriture par Marc Lauzon.
/// </summary>

namespace Thalass {
    [RequireComponent(typeof(Rigidbody))]
    public class SubmarineController : MonoBehaviour {

        [SerializeField]
        public Entities.Submarine m_submarine = null;

        [Space]
        [SerializeField]
        Camera m_camera = null;


        [Header("Movement")]
        [SerializeField]
        float m_energyConsumption = 0.01f;

        [SerializeField]
        float m_waterDrag = 0.1f;

        [SerializeField]
        float m_moveDeadZone = 0.2f;

        [Header("Interface")]
        [SerializeField]
        UI.MeterController m_batteryMeter = null;
        IDisposable m_batteryObserver = null;

        [SerializeField]
        UI.MeterController m_armorMeter = null;
        IDisposable m_armorObserver = null;

        Rigidbody m_rigidbody = null;
        Vector3 m_moveVelocity = Vector3.zero;
        Quaternion m_turnVelocity = Quaternion.identity;

        [SerializeField]
        LayerMask m_layerCollision;

        public AudioClip submarineEngineSound;
        
        void Start() {
            m_rigidbody = GetComponent<Rigidbody>();
            //m_rigidbody.drag = m_waterDrag;
            //m_rigidbody.angularDrag = m_waterDrag;

            m_batteryObserver = m_submarine.Battery.Subscribe(m_batteryMeter);
            m_armorObserver = m_submarine.Armor.Subscribe(m_armorMeter);
            m_submarine.Armor.Current = m_submarine.Armor.Maximum;
            m_submarine.Battery.Current = m_submarine.Battery.Maximum;
        }

        void OnDisable() {
            m_batteryObserver.Dispose();
            m_armorObserver.Dispose();
        }

        void FixedUpdate() {
            if (!this.isAlive()) {

                //Death move slowdown
                m_moveVelocity = Vector3.Lerp(m_moveVelocity, Vector3.zero, m_waterDrag);
                m_rigidbody.MovePosition(m_rigidbody.position + m_moveVelocity.normalized * m_moveVelocity.magnitude * Time.fixedDeltaTime / 10);

                // \Death turn slowdown
                m_turnVelocity = Quaternion.Lerp(m_turnVelocity, Quaternion.identity, m_waterDrag);
                m_rigidbody.rotation *= m_turnVelocity.normalized;
                return;
            }

            Turn();
            Move();


            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0 || Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftShift))
            {
                if (!SoundManager.instance.engineSource.isPlaying)
                    SoundManager.instance.PlaySingle(SoundManager.instance.engineSource, submarineEngineSound);
            }
            else
            {
                SoundManager.instance.StopMusic(SoundManager.instance.engineSource);
            }
        }

        void Update() {
            Vector3 p1 = transform.position + transform.forward;
            Vector3 p2 = transform.position - transform.forward;

            if(Physics.CapsuleCast(p1, p2, 0.75f, m_moveVelocity, out RaycastHit hit, 1, m_layerCollision)) {

                m_moveVelocity = Vector3.Reflect(m_moveVelocity, hit.normal);
                m_rigidbody.MovePosition(m_rigidbody.position + m_moveVelocity.normalized * m_submarine.Propulsion.Current * Time.deltaTime / 10);

            }
        }

        void Move() {
            //Get controls.
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Height"), Input.GetAxis("Vertical"));

            //Cummulate movement.
            m_moveVelocity += move.x * m_camera.transform.right + move.y * m_camera.transform.up + move.z * m_camera.transform.forward;

            //Calculate propulsion.
            m_submarine.Propulsion.Current = m_moveVelocity.magnitude * (int)m_submarine.Propulsion.Level;

            //Inaction slowdown.
            m_moveVelocity = Vector3.Lerp(m_moveVelocity, Vector3.zero, m_waterDrag);

            //Apply.
            //m_rigidbody.velocity = m_moveVelocity.normalized * m_submarine.Propulsion.Current * Time.fixedDeltaTime;
            m_rigidbody.MovePosition(m_rigidbody.position + m_moveVelocity.normalized * m_submarine.Propulsion.Current * Time.fixedDeltaTime / 10);

            //Consume energy on move.
            if (Vector3.Distance(move, Vector3.zero) > m_moveDeadZone) {
                m_submarine.Battery.Current -= m_energyConsumption;
            }
        }

        void Turn() {
            //Get controls.
            Quaternion turn = Quaternion.Euler(-Input.GetAxis("Mouse Y") * Time.deltaTime, Input.GetAxis("Mouse X") * Time.deltaTime, -Input.GetAxis("Roll") * Time.deltaTime);

            //Cummulate rotation.
            m_turnVelocity *= turn;

            //Inaction slowdown.                    
            m_turnVelocity = Quaternion.Lerp(m_turnVelocity, Quaternion.identity, m_waterDrag);

            //Apply.
            m_rigidbody.rotation *= m_turnVelocity.normalized;

            //Restore orientation.
            if (Quaternion.Angle(turn, Quaternion.identity) < m_moveDeadZone) {
                m_rigidbody.rotation = Quaternion.Lerp(m_rigidbody.rotation, Quaternion.Euler(0, m_rigidbody.rotation.eulerAngles.y, 0), m_waterDrag *2);
            }
        }

        public void GetDamaged(float _damage) {
            m_submarine.Armor.Current -= _damage;
            Debug.Log(m_submarine.Armor.Current);
            if (m_submarine.Armor.Current <= 0)
            {
                m_submarine.Battery.Current -= _damage;
            }

            //m_rigidbody.velocity *= 0f;
            //m_rigidbody.angularVelocity *= 0f;
        }

        public bool isAlive()
        {
            if (m_submarine.Battery.Current <= 0 && m_submarine.Armor.Current <= 0)
            {
                return false;
            }
            return true;
        }
    }
}
